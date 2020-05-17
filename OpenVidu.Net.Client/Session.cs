using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OpenVidu.Net.Client
{
    public class Session
    {
        private ILogger _logger;

        private string _sessionId;
        private long _createdAt;
        private OpenVidu _openVidu;
        private SessionProperties _properties;
        private ConcurrentDictionary<string, Connection> _activeConnections = new ConcurrentDictionary<string, Connection>();
        private bool _recording = false;

        public Session(OpenVidu openVidu)
        {
            _logger = ApplicationLogging.createLogger("Session");
            this._openVidu = openVidu;
            this._properties = new SessionPropertiesBuilder().build();
            Task.Run(async () => await getSessionIdHttp()).Wait();
        }

        public Session(OpenVidu openVidu, SessionProperties properties)
        {
            this._openVidu = openVidu;
            this._properties = properties;
            Task.Run(async () => await getSessionIdHttp()).Wait();
        }

        public Session(OpenVidu openVidu, JsonObject json)
        {
            this._openVidu = openVidu;
            this.resetSessionWithJson(json);
        }

        /**
	     * Gets the unique identifier of the Session
	     *
	     * @return The sessionId
	     */
        public string getSessionId()
        {
            return this._sessionId;
        }

        /**
	 * Timestamp when this session was created, in UTC milliseconds (ms since Jan 1,
	 * 1970, 00:00:00 UTC)
	 */
        public long createdAt()
        {
            return this._createdAt;
        }

        public bool hasSessionId()
        {
            return (!string.IsNullOrEmpty(_sessionId));
        }

        /**
	     * Gets a new token associated to Session object with default values for
	     * {@link io.openvidu.java.client.TokenOptions}. This always translates into a
	     * new request to OpenVidu Server
	     *
	     * @return The generated token
	     * 
	     * @throws OpenViduJavaClientException
	     * @throws OpenViduHttpException
	     */
        public async Task<string> generateToken() {
            return await this.generateToken(new TokenOptionsBuilder().role(OpenViduRole.PUBLISHER).build());
        }

        public async Task<string> generateToken(TokenOptions tokenOptions)
        {
            if (!this.hasSessionId())
            {
                this.getSessionId();
            }

            var json = new JsonObject
            {
                {"session", this._sessionId},
                {"role", tokenOptions.getRole().ToString()},
                {"data", tokenOptions.getData()}
            };
            if (tokenOptions.getKurentoOptions() != null)
            {
                var kurentoOptions = new JsonObject();
                if (tokenOptions.getKurentoOptions().VideoMaxRecvBandwidth != 0)
                {
                    kurentoOptions.Add("videoMaxRecvBandwidth",
                        tokenOptions.getKurentoOptions().VideoMaxRecvBandwidth);
                }
                if (tokenOptions.getKurentoOptions().VideoMinRecvBandwidth != 0)
                {
                    kurentoOptions.Add("videoMinRecvBandwidth",
                        tokenOptions.getKurentoOptions().VideoMinRecvBandwidth);
                }
                if (tokenOptions.getKurentoOptions().VideoMaxSendBandwidth != 0)
                {
                    kurentoOptions.Add("videoMaxSendBandwidth",
                        tokenOptions.getKurentoOptions().VideoMaxSendBandwidth);
                }
                if (tokenOptions.getKurentoOptions().VideoMinSendBandwidth != 0)
                {
                    kurentoOptions.Add("videoMinSendBandwidth",
                        tokenOptions.getKurentoOptions().VideoMinSendBandwidth);
                }
                if (tokenOptions.getKurentoOptions().AllowedFilters.Length > 0)
                {
                    var allowedFilters = new JsonArray();
                    foreach (var allowedFilter in tokenOptions.getKurentoOptions().AllowedFilters)
                    {
                        allowedFilters.Add(allowedFilter);
                    }
            
                    kurentoOptions.Add("allowedFilters", allowedFilters);
                }
                json.Add("kurentoOptions", kurentoOptions);
            }

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.PostAsync(_openVidu.ApiTokens, content);
            }
            catch (HttpRequestException ex)
            {
               throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await HttpResponseToJson(response);
                    responseJson.TryGetValue("id", out var idAsJsonValue);
                    var token = idAsJsonValue?.ToString();
                    _logger.LogInformation("Returning a TOKEN: {}", token);
                    return token;
                }
                else
                {
                    throw new OpenViduHttpException((int) response.StatusCode);
                }
            }
            catch (Exception e)
            { 
                throw new OpenViduClientException(e.Message);
            }

        }

        private async Task getSessionIdHttp()
        {
            if (this.hasSessionId())
            {
                return;
            }

            var json = new JsonObject
            {
                {"mediaMode", _properties.mediaMode().ToString()},
                {"recordingMode", _properties.recordingMode().ToString()},
                {"defaultOutputMode", _properties.defaultOutputMode().ToString()},
                {"defaultRecordingLayout", _properties.defaultRecordingLayout().ToString()},
                {"defaultCustomLayout", _properties.defaultCustomLayout()},
                {"customSessionId", _properties.customSessionId()}
            };

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.PostAsync(_openVidu.ApiSessions, content);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await HttpResponseToJson(response);
                    responseJson.TryGetValue("id", out var idAsJsonValue);
                    this._sessionId = idAsJsonValue;

                    responseJson.TryGetValue("createdAt", out var createdAtAsJsonValue);
                    long.TryParse(createdAtAsJsonValue?.ToString(), out _createdAt);

                    _logger.LogInformation("Session '{}' created", _sessionId);
                }
                else if (response.StatusCode.Equals(HttpStatusCode.Conflict))
                {
                    _sessionId = _properties.customSessionId();
                }
                else
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }
        }

        /**
	     * Gracefully closes the Session: unpublishes all streams and evicts every
	     * participant
	     * 
	     * @throws OpenViduJavaClientException
	     * @throws OpenViduHttpException
	     */
        public async Task<bool> close() {

            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.DeleteAsync(_openVidu.ApiSessions + "/" + _sessionId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.StatusCode.Equals(HttpStatusCode.NoContent))
                {
                    var session = this._openVidu.ActiveSessions.FirstOrDefault(s => s._sessionId == this._sessionId);
                    this._openVidu.ActiveSessions.Remove(session);
                    _logger.LogInformation("Session {} closed", this._sessionId);
                    return true;
                }
                else
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }

        }

        /**
	 * Returns the list of active connections to the session. <strong>This value
	 * will remain unchanged since the last time method
	 * {@link io.openvidu.java.client.Session#fetch()} was called</strong>.
	 * Exceptions to this rule are:
	 * <ul>
	 * <li>Calling {@link io.openvidu.java.client.Session#forceUnpublish(String)}
	 * updates each affected Connection status</li>
	 * <li>Calling {@link io.openvidu.java.client.Session#forceDisconnect(String)}
	 * updates each affected Connection status</li>
	 * </ul>
	 * <br>
	 * To get the list of active connections with their current actual value, you
	 * must call first {@link io.openvidu.java.client.Session#fetch()} and then
	 * {@link io.openvidu.java.client.Session#getActiveConnections()}
	 */
        public List<Connection> getActiveConnections()
        {
            return new List<Connection>(this._activeConnections.Values);
        }

        /**
	 * Updates every property of the Session with the current status it has in
	 * OpenVidu Server. This is especially useful for getting the list of active
	 * connections to the Session
	 * ({@link io.openvidu.java.client.Session#getActiveConnections()}) and use
	 * those values to call
	 * {@link io.openvidu.java.client.Session#forceDisconnect(Connection)} or
	 * {@link io.openvidu.java.client.Session#forceUnpublish(Publisher)}. <br>
	 * 
	 * To update every Session object owned by OpenVidu object, call
	 * {@link io.openvidu.java.client.OpenVidu#fetch()}
	 * 
	 * @return true if the Session status has changed with respect to the server,
	 *         false if not. This applies to any property or sub-property of the
	 *         object
	 * 
	 * @throws OpenViduHttpException
	 * @throws OpenViduJavaClientException
	 */
        public async Task<bool> fetch() 
        {
            var beforeJson = this.toJson();

            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.GetAsync(_openVidu.ApiSessions + "/" + this._sessionId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    this.resetSessionWithJson( await HttpResponseToJson(response));
                    var afterJson = this.toJson();
                    var hasChanged = !beforeJson.Equals(afterJson);
                    _logger.LogInformation("Session info fetched for session '{}'. Any change: {}", this._sessionId, hasChanged);
                    return hasChanged;
                }
                else
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }
        }

        public void forceDisconnect(Connection connection)
        {
            this.forceDisconnect(connection.ConnectionId);
        }

        public async void forceDisconnect(string connectionId)
        {
            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.DeleteAsync(_openVidu.ApiSessions + "/" + this._sessionId + "/connection/" + connectionId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Remove connection from activeConnections map
                    this._activeConnections.TryRemove(connectionId, out var connectionClosed);
                    // Remove every Publisher of the closed connection from every subscriber list of
                    // other connections
                    if (connectionClosed != null)
                    {
                        foreach (var publisher in connectionClosed.Publishers)
                        {
                            var streamId = publisher.StreamId;
                            foreach (var connection in this._activeConnections.Values)
                            {
                                connection.Subscribers = connection.Subscribers.Where(subscriber => !streamId.Equals(subscriber)).ToList();
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("The closed connection wasn't fetched in OpenVidu Java Client. No changes in the collection of active connections of the Session");
                    }
                    _logger.LogInformation("Connection {} closed", connectionId);
                }
                else
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }
        }

        public virtual void forceUnpublish(Publisher publisher)
        {
            this.forceUnpublish(publisher.StreamId);
        }

        public async void forceUnpublish(string streamId)
        {
            HttpResponseMessage response;

            try
            {
                response = await _openVidu.HttpClient.DeleteAsync(_openVidu.ApiSessions + "/" + this._sessionId + "/stream/" + streamId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    foreach (var connection in this._activeConnections.Values)
                    {
                        var publisher = connection.Publishers.Find(s => s.StreamId == streamId);
                        
                        // Try to remove the Publisher from the Connection publishers collection
                        if (connection.Publishers.Remove(publisher))
                        {
                            continue;
                        }
                        // Try to remove the Publisher from the Connection subscribers collection
                        connection.Subscribers.Remove(streamId);
                    }
                    _logger.LogInformation("Stream {} unpublished", streamId);
                }
                else
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }
        }

        public override string ToString()
        {
            return this._sessionId;
        }

        public bool IsBeingRecorded
        {
            set => _recording = value;
        }

        public Session resetSessionWithJson(JsonObject json)
        {
            json.TryGetValue("sessionId", out var sessionIdAsJsonValue);
            this._sessionId = sessionIdAsJsonValue;

            json.TryGetValue("createdAt", out var createdAtAsJsonValue);
            long.TryParse(createdAtAsJsonValue?.ToString(), out _createdAt);

            json.TryGetValue("recording", out var recordingAsJsonValue);
            bool.TryParse(recordingAsJsonValue?.ToString(), out _recording);

            json.TryGetValue("mediaMode", out var mediaModeAsJsonValue);
            Enum.TryParse(mediaModeAsJsonValue?.ToString(), out MediaMode mediaMode);

            json.TryGetValue("recordingMode", out var recordingModeAsJsonValue);
            Enum.TryParse(recordingModeAsJsonValue?.ToString(), out RecordingMode recordingMode);

            json.TryGetValue("defaultOutputMode", out var defaultOutputModeAsJsonValue);
            Enum.TryParse(defaultOutputModeAsJsonValue?.ToString(), out OutputMode outputMode);

            var builder = new SessionPropertiesBuilder()
                    .mediaMode(mediaMode)
                    .recordingMode(recordingMode)
                    .defaultOutputMode(outputMode);

            if (json.ContainsKey("defaultRecordingLayout"))
            {
                json.TryGetValue("defaultRecordingLayout", out var defaultRecordingLayoutAsJsonValue);
                Enum.TryParse(defaultRecordingLayoutAsJsonValue?.ToString(), out RecordingLayout recordingLayout);
                builder.defaultRecordingLayout(recordingLayout);
            }
            if (json.ContainsKey("defaultCustomLayout"))
            {
                json.TryGetValue("defaultCustomLayout", out var defaultCustomLayoutAsJsonValue);
                builder.defaultCustomLayout(defaultCustomLayoutAsJsonValue);
            }
            if (_properties?.customSessionId() != null)
            {
                builder.customSessionId(this._properties.customSessionId());
            }
            else if (json.ContainsKey("customSessionId"))
            {
                json.TryGetValue("customSessionId", out var customSessionIdAsJsonValue);
                builder.customSessionId(customSessionIdAsJsonValue);
            }
            this._properties = builder.build();


            json.TryGetValue("connections", out var connectionsAsJsonValue);
            var connectionsAsJsonObject = connectionsAsJsonValue as JsonObject;

            JsonValue contentAsJsonValue = null;
            connectionsAsJsonObject?.TryGetValue("content", out contentAsJsonValue);
           
            var jsonArrayConnections = contentAsJsonValue as JsonArray;
             

            this._activeConnections.Clear();
            jsonArrayConnections.ToList().ForEach(connection =>
            {
                var con = connection as JsonObject;

                var publishers = new Dictionary<string, Publisher>();

                con.TryGetValue("publishers", out var publishersAsJsonValue);
                var jsonArrayPublishers = publishersAsJsonValue as JsonArray;

                jsonArrayPublishers.ToList().ForEach(publisher =>
                {
                    var pubJson = publisher as JsonObject;
                    pubJson.TryGetValue("mediaOptions", out var mediaOptionsAsJsonValue);
                    var mediaOptions = mediaOptionsAsJsonValue as JsonObject;

                    pubJson.TryGetValue("streamId", out var streamIdAsJsonValue);

                    pubJson.TryGetValue("createdAt", out var createdatAsJsonValue);
                    long.TryParse(createdatAsJsonValue?.ToString(), out var createdAt);

                    mediaOptions.TryGetValue("hasAudio", out var hasAudioAsJsonValue);
                    bool.TryParse(hasAudioAsJsonValue?.ToString(), out var hasAudio);

                    mediaOptions.TryGetValue("hasVideo", out var hasVideoAsJsonValue);
                    bool.TryParse(hasVideoAsJsonValue?.ToString(), out var hasVideo);

                    mediaOptions.TryGetValue("audioActive", out var audioActiveAsJsonValue);
                    mediaOptions.TryGetValue("videoActive", out var videoActiveAsJsonValue);
                    mediaOptions.TryGetValue("typeOfVideo", out var typeOfVideoAsJsonValue);
                    mediaOptions.TryGetValue("videoDimensions", out var videoDimensionsAsJsonValue);

                    mediaOptions.TryGetValue("frameRate", out var frameRateAsJsonValue);
                    int.TryParse(frameRateAsJsonValue?.ToString(), out var frameRate);

                    var pub = new Publisher(streamIdAsJsonValue,
                        createdAt, hasAudio,
                        hasVideo, audioActiveAsJsonValue,
                        videoActiveAsJsonValue, frameRate, typeOfVideoAsJsonValue,
                        videoDimensionsAsJsonValue);

                    publishers.Add(pub.StreamId, pub);
                });

                var subscribers = new List<string>();
                con.TryGetValue("subscribers", out var subscribersAsJsonValue);


                var jsonArraySubscribers = subscribersAsJsonValue as JsonArray;

                jsonArraySubscribers.ToList().ForEach(subscriber =>
                {
                    var subscriberAsJsonObject = subscriber as JsonObject;
                    subscriberAsJsonObject.TryGetValue("streamId", out var streamIdAsJsonValue);
                    subscribers.Add(streamIdAsJsonValue);
                });

                con.TryGetValue("connectionId", out var connectionIdAsJsonValue);
                con.TryGetValue("createdAt", out var createdAtJsonValue);
                long.TryParse(createdAtJsonValue?.ToString(), out var createdat);
                con.TryGetValue("role", out var roleJsonValue);
                Enum.TryParse(roleJsonValue, out OpenViduRole role);
                con.TryGetValue("token", out var tokenAsJsonValue);
                con.TryGetValue("location", out var locationAsJsonValue);
                con.TryGetValue("platform", out var platformAsJsonValue);
                con.TryGetValue("serverData", out var serverDataAsJsonValue);
                con.TryGetValue("clientData", out var clientDataAsJsonValue);

                this._activeConnections.TryAdd(connectionIdAsJsonValue,
                    new Connection(connectionIdAsJsonValue, createdat,
                        role, tokenAsJsonValue,
                        locationAsJsonValue, platformAsJsonValue,
                        serverDataAsJsonValue, clientDataAsJsonValue, publishers,
                        subscribers));
        });

            return this;
        }

        private async Task<JsonObject> HttpResponseToJson(HttpResponseMessage response)
        {
            JsonObject json;

            try 
            {
                var content = await response.Content.ReadAsStringAsync();
                json = JsonValue.Parse(content) as JsonObject;
            } 
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message, e.InnerException);
            }

            return json;
        }

        protected string toJson()
        {
            var json = new JsonObject();
            json.Add("sessionId", this._sessionId);
            json.Add("createdAt", this._createdAt);
            json.Add("customSessionId", this._properties.customSessionId());
            json.Add("recording", this._recording);
            json.Add("mediaMode", this._properties.mediaMode().ToString());
            json.Add("recordingMode", this._properties.recordingMode().ToString());
            json.Add("defaultOutputMode", this._properties.defaultOutputMode().ToString());
            json.Add("defaultRecordingLayout", this._properties.defaultRecordingLayout().ToString());
            json.Add("defaultCustomLayout", this._properties.defaultCustomLayout());
            var connections = new JsonObject {{"numberOfElements", this._activeConnections.Count}};
            var jsonArrayConnections = new JsonArray();
            this.getActiveConnections().ForEach(con=>
            {
                var c = new JsonObject
                {
                    {"connectionId", con.ConnectionId},
                    {"role", con.Role.ToString()},
                    {"token", con.Token},
                    {"clientData", con.ClientData},
                    {"serverData", con.ServerData}
                };

                var pubs = new JsonArray();

                con.Publishers.ForEach(p=> {
                    pubs.Add(p.toJson());
                });

                var subs = new JsonArray();
                con.Subscribers.ForEach(s=> 
                {
                    subs.Add(s);
                });
                c.Add("publishers", pubs);
                c.Add("subscribers", subs);
                jsonArrayConnections.Add(c);
            });
            connections.Add("content", jsonArrayConnections);
            json.Add("connections", connections);
            return json.ToString();
        }
    }
}

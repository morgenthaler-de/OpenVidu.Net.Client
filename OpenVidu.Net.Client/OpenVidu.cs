using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace OpenVidu.Net.Client
{
    public class OpenVidu
    {
        private ILogger _logger;

        private string _secret;
        public HttpClient HttpClient { get; }
        private ConcurrentDictionary<string, Session> _activeSessions = new ConcurrentDictionary<string, Session>();

        public string ApiSessions => "api/sessions";

        public string ApiTokens => "api/tokens";
        public string ApiRecordings => "api/recordings";
        public string ApiRecordingsStart => "/start";
        public string ApiRecordingsStop => "/stop";

        public OpenVidu(string hostname, string secret)
        {
            _logger = ApplicationLogging.createLogger("OpenVidu");

            this.Hostname = hostname;
            if (!this.Hostname.EndsWith("/"))
            {
                this.Hostname += "/";
            }

            this._secret = secret;

            HttpClient = new HttpClient();
            var authInfo = $"OPENVIDUAPP:{this._secret}";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
            HttpClient.BaseAddress = new Uri(Hostname);
            HttpClient.Timeout = TimeSpan.FromMilliseconds(300000);
        }

        public string Hostname { get; }

        public IList<Session> ActiveSessions => new List<Session>(this._activeSessions.Values);

        /// <summary>
        /// Creates an OpenVidu session with the default settings
        /// </summary>
        /// <returns> The created session
        /// </returns>
        /// <exception cref="OpenViduClientException"> </exception>
        /// <exception cref="OpenViduHttpException"> </exception>
        public Session createSession()
        {
            var session = new Session(this);
            this._activeSessions[session.getSessionId()] = session;
            return session;
        }

        /// <summary>
        /// Creates an OpenVidu session
        /// </summary>
        /// <param name="properties"> The specific configuration for this session
        /// </param>
        /// <returns> The created session
        /// </returns>
        /// <exception cref="OpenViduClientException"> </exception>
        /// <exception cref="OpenViduHttpException">
        /// Value returned from <seealso cref="OpenViduHttpException.Status"/>
        ///<ul>
        ///<li><code>409</code>: you are trying to
        ///assign an already-in-use custom sessionId
        ///to the session. See
        ///<seealso cref="SessionProperties.customSessionId()"/></li>
        ///</ul> </exception>
        public Session createSession(SessionProperties properties)
        {
            var session = new Session(this, properties);
            this._activeSessions[session.getSessionId()] = session;
            return session;
        }

        public List<Session> getActiveSessions()
        {
            return new List<Session>(this._activeSessions.Values);
        }

        public async Task<Recording> startRecording(string sessionId, RecordingProperties properties)
        {
            HttpResponseMessage response;

            var json = new JsonObject
            {
                {"session", sessionId},
                {"name", properties.Name},
                {"outputMode", properties.OutputMode.ToString()},
                {"hasAudio", properties.HasAudio},
                {"hasVideo", properties.HasVideo}
            };

            if (OutputMode.COMPOSED.Equals(properties.OutputMode) && properties.HasVideo)
            {
                json.Add("resolution", properties.Resolution);
                json.Add("recordingLayout", properties.RecordingLayout > 0 ? properties.RecordingLayout.ToString() : "");
                if (RecordingLayout.CUSTOM.Equals(properties.RecordingLayout))
                {
                    json.Add("customLayout", !string.IsNullOrEmpty(properties.CustomLayout) ? properties.CustomLayout : "");
                }
            }

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                response = await HttpClient.PostAsync(ApiRecordings + ApiRecordingsStart, content);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var r = new Recording(await httpResponseToJson(response));
                    this._activeSessions.TryGetValue(r.SessionId, out var activeSession);

                    if (activeSession != null)
                    {
                        activeSession.IsBeingRecorded = true;
                    }
                    else
                    {
                        _logger.LogWarning("No active session found for sessionId '" + r.SessionId + "'. This instance of OpenVidu Client didn't create this session");
                    }
                    return r;
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

        public async Task<Recording> startRecording(string sessionId, string name)
        {
            if (string.ReferenceEquals(name, null))
            {
                name = "";
            }
            return await startRecording(sessionId, (new RecordingPropertiesBuilder()).name(name).build());
        }

        public async Task<Recording> startRecording(string sessionId)
        {
            return await startRecording(sessionId, "");
        }

        public async Task<Recording> stopRecording(string recordingId)
        {
            HttpResponseMessage response;

            var content = new StringContent("", Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                response = await HttpClient.PostAsync(ApiRecordings + ApiRecordingsStop + "/" + recordingId, content);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var r = new Recording(await httpResponseToJson(response));
                    this._activeSessions.TryGetValue(r.SessionId, out var activeSession);

                    if (activeSession != null)
                    {
                        activeSession.IsBeingRecorded = false;
                    }
                    else
                    {
                        _logger.LogWarning("No active session found for sessionId '" + r.SessionId + "'. This instance of OpenVidu Client didn't create this session");
                    }
                    return r;
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

        public async Task<Recording> getRecording(string recordingId)
        {
            HttpResponseMessage response;

            try
            {
                response = await HttpClient.GetAsync(ApiRecordings + "/" + recordingId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return new Recording(await httpResponseToJson(response));
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

        public async Task<IList<Recording>> listRecordings()
        {
            HttpResponseMessage response;

            try
            {
                response = await HttpClient.GetAsync(ApiRecordings);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<Recording> recordings = new List<Recording>();
                    var json = await httpResponseToJson(response);
                    json.TryGetValue("items", out var array);
                    if (array == null) return recordings;
                    foreach (var item in array)
                    {
                        recordings.Add(new Recording(item as JsonObject));
                    }
                    return recordings;
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

        public async void deleteRecordings(string recordingId)
        {
            HttpResponseMessage response;

            try
            {
                response = await HttpClient.DeleteAsync(ApiRecordings + "/" + recordingId);
            }
            catch (HttpRequestException ex)
            {
                throw new OpenViduClientException(ex.Message, ex.InnerException);
            }

            try
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new OpenViduHttpException((int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new OpenViduClientException(e.Message);
            }
        }

        private async Task<JsonObject> httpResponseToJson(HttpResponseMessage response)
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
    }
}

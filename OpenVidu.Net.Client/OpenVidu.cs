using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    }
}

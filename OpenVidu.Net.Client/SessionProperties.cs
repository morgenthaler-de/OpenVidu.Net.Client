namespace OpenVidu.Net.Client
{
    public class SessionProperties
    {
        private readonly MediaMode _mediaMode;
        private readonly RecordingMode _recordingMode;
        private readonly OutputMode _defaultOutputMode;
        private readonly RecordingLayout _defaultRecordingLayout;
        private readonly string _defaultCustomLayout;
        private readonly string _customSessionId;

        public SessionProperties(MediaMode mediaMode, RecordingMode recordingMode, OutputMode outputMode,
            RecordingLayout layout, string defaultCustomLayout, string customSessionId)
        {
            this._mediaMode = mediaMode;
            this._recordingMode = recordingMode;
            this._defaultOutputMode = outputMode;
            this._defaultRecordingLayout = layout;
            this._defaultCustomLayout = defaultCustomLayout;
            this._customSessionId = customSessionId;
        }

		/**
	 * Defines how the media streams will be sent and received by your clients:
	 * routed through OpenVidu Media Node (<code>MediaMode.ROUTED</code>) or
	 * attempting direct p2p connections (<code>MediaMode.RELAYED</code>, <i>not
	 * available yet</i>)
	 */
		public MediaMode mediaMode()
		{
			return this._mediaMode;
		}

		/**
		 * Defines whether the Session will be automatically recorded
		 * ({@link RecordingMode#ALWAYS}) or not ({@link RecordingMode#MANUAL})
		 */
		public RecordingMode recordingMode()
		{
			return this._recordingMode;
		}

		/**
		 * Defines the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#outputMode()} of every
		 * recording of this session. You can easily override this value later when
		 * starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#outputMode(Recording.OutputMode)}
		 * with any other value
		 */
		public OutputMode defaultOutputMode()
		{
			return this._defaultOutputMode;
		}

		/**
		 * Defines the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#recordingLayout()} of
		 * every recording of this session. You can easily override this value later
		 * when starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#recordingLayout(RecordingLayout)}
		 * with any other value.<br>
		 * Recording layouts are only applicable to recordings with OutputMode
		 * {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED}
		 */
		public RecordingLayout defaultRecordingLayout()
		{
			return this._defaultRecordingLayout;
		}

		/**
		 * Defines the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#customLayout()} of every
		 * recording of this session. You can easily override this value later when
		 * starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#customLayout(String)}
		 * with any other value.<br>
		 * CUSTOM layouts are only applicable to recordings with OutputMode
		 * {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED} and
		 * RecordingLayout {@link io.openvidu.java.client.RecordingLayout#CUSTOM}
		 */
		public string defaultCustomLayout()
		{
			return this._defaultCustomLayout;
		}

		/**
		 * Fixes the value of the sessionId property of the Session. You can take
		 * advantage of this property to facilitate the mapping between OpenVidu Server
		 * 'session' entities and your own 'session' entities. If this parameter is
		 * undefined or an empty string, OpenVidu Server will generate a random
		 * sessionId for you.
		 */
		public string customSessionId()
		{
			return this._customSessionId;
		}
	}
}

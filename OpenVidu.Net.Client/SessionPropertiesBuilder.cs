namespace OpenVidu.Net.Client
{
    public class SessionPropertiesBuilder
    {
        private MediaMode _mediaMode = MediaMode.ROUTED;
        private RecordingMode _recordingMode = RecordingMode.MANUAL;
        private OutputMode _defaultOutputMode = OutputMode.COMPOSED;
        private RecordingLayout _defaultRecordingLayout = RecordingLayout.BEST_FIT;
        private string _defaultCustomLayout = string.Empty;
        private string _customSessionId = string.Empty;

        /**
		 * Returns the {@link io.openvidu.java.client.SessionProperties} object properly
		 * configured
		 */
        public SessionProperties build()
        {
            return new SessionProperties(this._mediaMode, this._recordingMode, this._defaultOutputMode,
                this._defaultRecordingLayout, this._defaultCustomLayout, this._customSessionId);
        }

		/**
		 * Call this method to set how the media streams will be sent and received by
		 * your clients: routed through OpenVidu Media Node
		 * (<code>MediaMode.ROUTED</code>) or attempting direct p2p connections
		 * (<code>MediaMode.RELAYED</code>, <i>not available yet</i>)
		 * 
		 * Default value is <code>MediaMode.ROUTED</code>
		 */
		public SessionPropertiesBuilder mediaMode(MediaMode mediaMode)
		{
			this._mediaMode = mediaMode;
			return this;
		}

		/**
		 * Call this method to set whether the Session will be automatically recorded
		 * ({@link RecordingMode#ALWAYS}) or not ({@link RecordingMode#MANUAL})<br>
		 * Default value is {@link RecordingMode#MANUAL}
		 */
		public SessionPropertiesBuilder recordingMode(RecordingMode recordingMode)
		{
			this._recordingMode = recordingMode;
			return this;
		}

		/**
		 * Call this method to set the the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#outputMode()} of every
		 * recording of this session. You can easily override this value later when
		 * starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#outputMode(Recording.OutputMode)}
		 * with any other value.<br>
		 * Default value is {@link Recording.OutputMode#COMPOSED}
		 */
		public SessionPropertiesBuilder defaultOutputMode(OutputMode outputMode)
		{
			this._defaultOutputMode = outputMode;
			return this;
		}

		/**
		 * Call this method to set the the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#recordingLayout()} of
		 * every recording of this session. You can easily override this value later
		 * when starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#recordingLayout(RecordingLayout)}
		 * with any other value.<br>
		 * Default value is {@link RecordingLayout#BEST_FIT}<br>
		 * <br>
		 * Recording layouts are only applicable to recordings with OutputMode
		 * {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED}
		 */
		public SessionPropertiesBuilder defaultRecordingLayout(RecordingLayout layout)
		{
			this._defaultRecordingLayout = layout;
			return this;
		}

		/**
		 * Call this method to set the default value used to initialize property
		 * {@link io.openvidu.java.client.RecordingProperties#customLayout()} of every
		 * recording of this session. You can easily override this value later when
		 * starting a {@link io.openvidu.java.client.Recording} by calling
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#customLayout(String)}
		 * with any other value.<br>
		 * <br>
		 * 
		 * CUSTOM layouts are only applicable to recordings with OutputMode
		 * {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED} and
		 * RecordingLayout {@link io.openvidu.java.client.RecordingLayout#CUSTOM}
		 */
		public SessionPropertiesBuilder defaultCustomLayout(string path)
		{
			this._defaultCustomLayout = path;
			return this;
		}

		/**
		 * Call this method to fix the sessionId that will be assigned to the session.
		 * You can take advantage of this property to facilitate the mapping between
		 * OpenVidu Server 'session' entities and your own 'session' entities. If this
		 * parameter is undefined or an empty string, OpenVidu Server will generate a
		 * random sessionId for you.
		 */
		public SessionPropertiesBuilder customSessionId(string customSessionId)
		{
			this._customSessionId = customSessionId;
			return this;
		}

	}
}

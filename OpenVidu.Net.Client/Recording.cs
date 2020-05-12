using System;
using System.Json;
using RecordingStatus = OpenVidu.Net.Client.Status;

namespace OpenVidu.Net.Client
{
    public class Recording
    {
        private readonly RecordingProperties _recordingProperties;

        public Recording(JsonObject json)
        {
            json.TryGetValue("id", out var idAsJsonValue);
            Id = idAsJsonValue?.ToString();

            json.TryGetValue("name", out var nameAsJsonValue);
            var name = nameAsJsonValue?.ToString();

            json.TryGetValue("resolution", out var resolutionAsJsonValue);
            var resolution = resolutionAsJsonValue?.ToString();

            json.TryGetValue("sessionId", out var sessionIdAsJsonValue);
            SessionId = sessionIdAsJsonValue?.ToString();

            json.TryGetValue("createdAt", out var createdAtAsJsonValue);
            long.TryParse(createdAtAsJsonValue?.ToString(), out var createdAt);
            CreatedAt = createdAt;

            json.TryGetValue("size", out var sizeAsJsonValue);
            long.TryParse(sizeAsJsonValue?.ToString(), out var size);
            Size = size;

            json.TryGetValue("duration", out var durationAsJsonValue);
            double.TryParse(durationAsJsonValue?.ToString(), out var duration);
            Duration = duration;

           json.TryGetValue("url", out var urlAsJsonValue);
           Url = urlAsJsonValue?.ToString();

           json.TryGetValue("status", out var statusAsJsonValue);
           Enum.TryParse(statusAsJsonValue?.ToString(), out RecordingStatus status);
           Status = status;

           json.TryGetValue("hasAudio", out var hasAudioAsJsonValue);
           bool.TryParse(hasAudioAsJsonValue?.ToString(), out var hasAudio);

           json.TryGetValue("hasVideo", out var hasVideoAsJsonValue);
           bool.TryParse(hasVideoAsJsonValue?.ToString(), out var hasVideo);

           json.TryGetValue("outputMode", out var outputModeAsJsonValue);
           Enum.TryParse(outputModeAsJsonValue?.ToString(), out OutputMode outputMode);

           json.TryGetValue("recordingLayout", out var recordingLayoutAsJsonValue);
           Enum.TryParse(recordingLayoutAsJsonValue?.ToString(), out RecordingLayout recordingLayout);

           json.TryGetValue("customLayout", out var customLayoutAsJsonValue);
          
           var builder = new RecordingPropertiesBuilder().name(name).outputMode(outputMode).hasAudio(hasAudio).hasVideo(hasVideo);

           if (OutputMode.COMPOSED.Equals(outputMode) && hasVideo)
           {
               builder.resolution(resolution).recordingLayout(recordingLayout);
               if (!string.IsNullOrEmpty(customLayoutAsJsonValue.ToString()))
               {
                   builder.customLayout(customLayoutAsJsonValue.ToString());
               }
           }

           this._recordingProperties = builder.build();
        }

		/**
	 * Status of the recording
	 */
		public Status Status { get; }

		/**
		 * Recording unique identifier
		 */
		public string Id { get; }

        /**
         * Name of the recording. The video file will be named after this property. You
         * can access this same value in your clients on recording events
         * (<code>recordingStarted</code>, <code>recordingStopped</code>)
         */
		public string Name => this._recordingProperties.Name;

        /**
         * Mode of recording: COMPOSED for a single archive in a grid layout or
         * INDIVIDUAL for one archive for each stream
         */
		public OutputMode OutputMode => this._recordingProperties.OutputMode;

        /**
         * The layout used in this recording. Only defined if OutputMode is COMPOSED
         */
        public RecordingLayout RecordingLayout => this._recordingProperties.RecordingLayout;

        /**
         * The custom layout used in this recording. Only defined if if OutputMode is
         * COMPOSED and
         * {@link io.openvidu.java.client.RecordingProperties.Builder#customLayout(String)}
         * has been called
         */
        public string CustomLayout => this._recordingProperties.CustomLayout;

        /**
         * Session associated to the recording
         */
        public string SessionId { get; }

        /**
         * Time when the recording started in UTC milliseconds
         */
		public long CreatedAt { get; }

		/**
         * Size of the recording in bytes (0 until the recording is stopped)
         */
		public long Size { get; }

		/**
         * Duration of the recording in seconds (0 until the recording is stopped)
         */
		public double Duration { get; }

        /**
         * URL of the recording. You can access the file from there. It is
         * <code>null</code> until recording reaches "ready" or "failed" status. If
         * <a href="https://docs.openvidu.io/en/stable/reference-docs/openvidu-server-params/"
         * target="_blank">OpenVidu Server configuration</a> property
         * <code>openvidu.recording.public-access</code> is false, this path will be
         * secured with OpenVidu credentials
         */
		public string Url { get; }

        /**
         * Resolution of the video file. Only defined if OutputMode of the Recording is
         * set to {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED}
         */
		public string Resolution => this._recordingProperties.Resolution;

        /**
         * <code>true</code> if the recording has an audio track, <code>false</code>
         * otherwise (currently fixed to true)
         */
        public bool HasAudio => this._recordingProperties.HasAudio;

        /**
         * <code>true</code> if the recording has a video track, <code>false</code>
         * otherwise (currently fixed to true)
         */
        public bool HasVideo => this._recordingProperties.HasVideo;
    }
}

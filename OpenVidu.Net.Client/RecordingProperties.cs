using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public class RecordingProperties
    {
        public RecordingProperties(string name, OutputMode outputMode, RecordingLayout layout,
            string customLayout, string resolution, bool hasAudio, bool hasVideo)
        {
            this.Name = name;
            this.OutputMode = outputMode;
            this.RecordingLayout = layout;
            this.CustomLayout = customLayout;
            this.Resolution = resolution;
            this.HasAudio = hasAudio;
            this.HasVideo = hasVideo;
        }

		/**
	 * Defines the name you want to give to the video file. You can access this same
	 * value in your clients on recording events (<code>recordingStarted</code>,
	 * <code>recordingStopped</code>)
	 */
		public string Name
        { get; }

		/**
		 * Defines the mode of recording: {@link Recording.OutputMode#COMPOSED} for a
		 * single archive in a grid layout or {@link Recording.OutputMode#INDIVIDUAL}
		 * for one archive for each stream.<br>
		 * <br>
		 * 
		 * Default to {@link Recording.OutputMode#COMPOSED}
		 */
		public OutputMode OutputMode
		{ get; }

		/**
		 * Defines the layout to be used in the recording.<br>
		 * Will only have effect if
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#outputMode(Recording.OutputMode)}
		 * has been called with value {@link Recording.OutputMode#COMPOSED}.<br>
		 * <br>
		 * 
		 * Default to {@link RecordingLayout#BEST_FIT}
		 */
		public RecordingLayout RecordingLayout
		{ get; }

		/**
		 * If {@link io.openvidu.java.client.RecordingProperties#recordingLayout()} is
		 * set to {@link io.openvidu.java.client.RecordingLayout#CUSTOM}, this property
		 * defines the relative path to the specific custom layout you want to use.<br>
		 * See <a href=
		 * "https://docs.openvidu.io/en/stable/advanced-features/recording#custom-recording-layouts"
		 * target="_blank">CUSTOM recording layouts</a> to learn more
		 */
		public string CustomLayout
		{ get; }

		/**
		 * Defines the resolution of the recorded video.<br>
		 * Will only have effect if
		 * {@link io.openvidu.java.client.RecordingProperties.Builder#outputMode(Recording.OutputMode)}
		 * has been called with value
		 * {@link io.openvidu.java.client.Recording.OutputMode#COMPOSED}. For
		 * {@link io.openvidu.java.client.Recording.OutputMode#INDIVIDUAL} all
		 * individual video files will have the native resolution of the published
		 * stream.<br>
		 * <br>
		 * 
		 * Default to "1920x1080"
		 */
		public string Resolution
		{ get; }

		/**
		 * Defines whether to record audio or not. Cannot be set to false at the same
		 * time as {@link hasVideo()}.<br>
		 * <br>
		 * 
		 * Default to true
		 */
		public bool HasAudio
		{ get; }

		/**
		 * Defines whether to record video or not. Cannot be set to false at the same
		 * time as {@link hasAudio()}.<br>
		 * <br>
		 * 
		 * Default to true
		 */
		public bool HasVideo
		{ get; }
	}
}

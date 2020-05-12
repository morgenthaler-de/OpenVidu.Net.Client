using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenVidu.Net.Client
{
    public class RecordingPropertiesBuilder
    {
        private string _name = string.Empty;
        private OutputMode _outputMode = OutputMode.COMPOSED;
        private RecordingLayout _recordingLayout;
        private string _customLayout = string.Empty;
        private string _resolution = string.Empty;
        private bool _hasAudio = true;
        private bool _hasVideo = true;


        public RecordingProperties build()
        {
            if (!OutputMode.COMPOSED.Equals(this._outputMode))
                return new RecordingProperties(this._name, this._outputMode, this._recordingLayout, this._customLayout,
                    this._resolution, this._hasAudio, this._hasVideo);

            this._recordingLayout = this._recordingLayout != 0 ? this._recordingLayout : RecordingLayout.BEST_FIT;
            this._resolution = !string.IsNullOrEmpty(_resolution) ? this._resolution : "1920x1080";
            if (RecordingLayout.CUSTOM.Equals(this._recordingLayout))
            {
                this._customLayout = !string.IsNullOrEmpty(_customLayout) ? this._customLayout : "";
            }

            return new RecordingProperties(this._name, this._outputMode, this._recordingLayout, this._customLayout,
                this._resolution, this._hasAudio, this._hasVideo);
        }

        public RecordingPropertiesBuilder name(string name )
        {
            this._name = name;
            return this;
        }

        public RecordingPropertiesBuilder outputMode(OutputMode outputMode)
        {
            this._outputMode = outputMode;
            return this;
        }

        public RecordingPropertiesBuilder recordingLayout(RecordingLayout recordingLayout)
        {
            this._recordingLayout = recordingLayout;
            return this;
        }

        public RecordingPropertiesBuilder customLayout(string path)
        {
            this._customLayout = path;
            return this;
        }

        public RecordingPropertiesBuilder resolution(string resolution)
        {
            this._resolution = resolution;
            return this;
        }

        public RecordingPropertiesBuilder hasAudio(bool hasAudio)
        {
            this._hasAudio = hasAudio;
            return this;
        }

        public RecordingPropertiesBuilder hasVideo(bool hasVideo)
        {
            this._hasVideo = hasVideo;
            return this;
        }
    }
}

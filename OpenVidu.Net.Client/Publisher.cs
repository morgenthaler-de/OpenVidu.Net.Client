using System;
using System.Collections.Generic;
using System.Json;
using System.Text;
using System.Text.Json;

namespace OpenVidu.Net.Client
{
    public class Publisher
    {

        public Publisher(string streamId, long createdAt, bool hasAudio, bool hasVideo, bool audioActive, 
            bool videoActive, int frameRate, string typeOfVideo, string videoDimensions)
        {
            this.StreamId = streamId;
            this.CreatedAt = createdAt;
            this.HasAudio = hasAudio;
            this.HasVideo = hasVideo;
            this.IsAudioActive = audioActive;
            this.IsVideoActive = videoActive;
            this.FrameRate = frameRate;
            this.TypeOfVideo = typeOfVideo;
            this.VideoDimensions = videoDimensions;

        }

        /**
	 * Returns the unique identifier of the
	 * <a href="/api/openvidu-browser/classes/stream.html" target=
	 * "_blank">Stream</a> associated to this PUBLISHER. Each PUBLISHER is paired
	 * with only one Stream, so you can identify each PUBLISHER by its
	 * <a href="/api/openvidu-browser/classes/stream.html#streamid" target=
	 * "_blank"><code>Stream.streamId</code></a>
	 */
        public string StreamId
        {
            get;
        }

        /**
     * Timestamp when this PUBLISHER started publishing, in UTC milliseconds (ms
     * since Jan 1, 1970, 00:00:00 UTC)
     */
        public long CreatedAt
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public bool HasVideo
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public bool HasAudio
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public bool IsAudioActive
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public bool IsVideoActive
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public int FrameRate
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public string TypeOfVideo
        {
            get;
        }

        /**
         * See properties of <a href="/api/openvidu-browser/classes/stream.html" target=
         * "_blank">Stream</a> object in OpenVidu Browser library to find out more
         */
        public string VideoDimensions
        {
            get;
        }

        public JsonObject toJson()
        {
            var json = new JsonObject
            {
                {"streamId", this.StreamId},
                {"hasAudio", this.HasAudio},
                {"hasVideo", this.HasVideo},
                {"audioActive", this.IsAudioActive},
                {"videoActive", this.IsVideoActive},
                {"frameRate", this.FrameRate},
                {"typeOfVideo", this.TypeOfVideo},
                {"videoDimensions", this.VideoDimensions}
            };
            return json;
        }
    }
}

using System;
using System.Dynamic;

namespace OpenVidu.Net.Client
{
    public class KurentoOptions
    {
        public KurentoOptions(int videoMaxRecvBandwidth, int videoMinRecvBandwidth, int videoMaxSendBandwidth,
				int videoMinSendBandwidth, string[] allowedFilters)
		{
			this.VideoMaxRecvBandwidth = videoMaxRecvBandwidth;
			this.VideoMinRecvBandwidth = videoMinRecvBandwidth;
			this.VideoMaxSendBandwidth = videoMaxSendBandwidth;
			this.VideoMinSendBandwidth = videoMinSendBandwidth;
			this.AllowedFilters = allowedFilters;
		}

		/**
		 * Defines the maximum number of Kbps that the client owning the token will be
		 * able to receive from Kurento Media Server. 0 means unconstrained. Giving a
		 * value to this property will override the global configuration set in <a href=
		 * "https://docs.openvidu.io/en/stable/reference-docs/openvidu-server-params/#configuration-parameters-for-openvidu-server"
		 * target="_blank">OpenVidu Server configuration</a> (parameter
		 * <code>openvidu.streams.video.max-recv-bandwidth</code>) for every incoming
		 * stream of the user owning the token. <br>
		 * <strong>WARNING</strong>: the lower value set to this property limits every
		 * other bandwidth of the WebRTC pipeline this server-to-client stream belongs
		 * to. This includes the user publishing the stream and every other user
		 * subscribed to the stream
		 */
		public int VideoMaxRecvBandwidth
        { get; }

		/**
		 * Defines the minimum number of Kbps that the client owning the token will try
		 * to receive from Kurento Media Server. 0 means unconstrained. Giving a value
		 * to this property will override the global configuration set in <a href=
		 * "https://docs.openvidu.io/en/stable/reference-docs/openvidu-server-params/#configuration-parameters-for-openvidu-server"
		 * target="_blank">OpenVidu Server configuration</a> (parameter
		 * <code>openvidu.streams.video.min-recv-bandwidth</code>) for every incoming
		 * stream of the user owning the token.
		 */
		public int VideoMinRecvBandwidth
        { get; }

		/**
		 * Defines the maximum number of Kbps that the client owning the token will be
		 * able to send to Kurento Media Server. 0 means unconstrained. Giving a value
		 * to this property will override the global configuration set in <a href=
		 * "https://docs.openvidu.io/en/stable/reference-docs/openvidu-server-params/#configuration-parameters-for-openvidu-server"
		 * target="_blank">OpenVidu Server configuration</a> (parameter
		 * <code>openvidu.streams.video.max-send-bandwidth</code>) for every outgoing
		 * stream of the user owning the token. <br>
		 * <strong>WARNING</strong>: this value limits every other bandwidth of the
		 * WebRTC pipeline this client-to-server stream belongs to. This includes every
		 * other user subscribed to the stream
		 */
		public int VideoMaxSendBandwidth
        { get; }

		/**
		 * Defines the minimum number of Kbps that the client owning the token will try
		 * to send to Kurento Media Server. 0 means unconstrained. Giving a value to
		 * this property will override the global configuration set in <a href=
		 * "https://docs.openvidu.io/en/stable/reference-docs/openvidu-server-params/#configuration-parameters-for-openvidu-server"
		 * target="_blank">OpenVidu Server configuration</a> (parameter
		 * <code>openvidu.streams.video.min-send-bandwidth</code>) for every outgoing
		 * stream of the user owning the token.
		 */
		public int VideoMinSendBandwidth
        { get; }

		/**
		 * Defines the names of the filters the user owning the token will be able to
		 * apply. See
		 * <a href="https://docs.openvidu.io/en/stable/advanced-features/filters/" target=
		 * "_blank">Voice and video filters</a>
		 */
		public string[] AllowedFilters
        { get; }
	}

    
}

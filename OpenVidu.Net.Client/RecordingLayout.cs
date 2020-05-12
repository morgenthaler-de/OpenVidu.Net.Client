using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public enum RecordingLayout
    {
        NULL,

        /**
         * All the videos are evenly distributed, taking up as much space as possible
         */
        BEST_FIT,

        /**
         * <i>(not available yet)</i>
         */
        PICTURE_IN_PICTURE,

        /**
         * <i>(not available yet)</i>
         */
        VERTICAL_PRESENTATION,

        /**
         * <i>(not available yet)</i>
         */
        HORIZONTAL_PRESENTATION,

        /**
         * Use your own custom recording layout. See <a href=
         * "https://docs.openvidu.io/en/stable/advanced-features/recording#custom-recording-layouts"
         * target="_blank">CUSTOM recording layouts</a> to learn more
         */
        CUSTOM
	}
}

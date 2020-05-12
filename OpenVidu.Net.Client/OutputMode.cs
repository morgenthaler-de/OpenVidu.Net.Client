using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public enum OutputMode
    {
        NULL,
        /**
         * Record all streams in a grid layout in a single archive
         */
        COMPOSED,

        /**
         * Record each stream individually
         */
        INDIVIDUAL
	}
}

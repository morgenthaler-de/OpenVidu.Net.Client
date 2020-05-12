using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public enum MediaMode
    {
        NULL,
        /**
     * <i>(not available yet)</i> The session will attempt to transmit streams
     * directly between clients
     */
        RELAYED,

        /**
         * The session will transmit streams using OpenVidu Media Node
         */
        ROUTED
	}
}

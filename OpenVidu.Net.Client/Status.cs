using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public enum Status
    {
        Null,

        /**
         * The recording is starting (cannot be stopped). Some recording may not go
         * through this status and directly reach "started" status
         */
        starting,

        /**
         * The recording has started and is going on
         */
        started,

        /**
         * The recording has stopped and is being processed. At some point it will reach
         * "ready" status
         */
        stopped,

        /**
         * The recording has finished OK and is available for download through OpenVidu
         * Server recordings endpoint:
         * https://YOUR_OPENVIDUSERVER_IP/recordings/{RECORDING_ID}/{RECORDING_NAME}.{EXTENSION}
         */
        ready,

        /**
         * The recording has failed. This status may be reached from "starting",
         * "started" and "stopped" status
         */
        failed
	}
}

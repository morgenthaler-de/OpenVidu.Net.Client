using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public enum RecordingMode
    {
        NULL,
        /**
     * The session is recorded automatically as soon as the first client publishes a
     * stream to the session. It is automatically stopped after last user leaves the
     * session (or until you call
     * {@link io.openvidu.java.client.OpenVidu#stopRecording(String)}).
     */
        ALWAYS,

        /**
         * The session is not recorded automatically. To record the session, you must
         * call {@link io.openvidu.java.client.OpenVidu#startRecording(String)} method.
         * To stop the recording, you must call
         * {@link io.openvidu.java.client.OpenVidu#stopRecording(String)}.
         */
        MANUAL
	}
}

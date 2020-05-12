using System;

namespace OpenVidu.Net.Client
{
    /// <summary>
    /// Defines a generic OpenVidu exception
    /// </summary>
    public class OpenViduException : Exception
    {
        public static long SerialVersionUid { get; } = 1L;

        public OpenViduException(string message) : base(message)
        {
        }

        public OpenViduException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    /// <summary>
    /// Defines unexpected internal errors in OpenVidu Java Client
    /// </summary>
    public class OpenViduClientException : OpenViduException
    {

        public OpenViduClientException(string message) : base(message) {}

        public OpenViduClientException(string message, Exception cause) : base(message, cause) { }
    }
}

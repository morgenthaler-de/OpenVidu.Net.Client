namespace OpenVidu.Net.Client
{
    /// <summary>
    /// Defines error responses from OpenVidu Server
    /// </summary>
    public class OpenViduHttpException : OpenViduException
    {
        public OpenViduHttpException(int status) : base(status.ToString())
        {
            this.Status = status;
        }

        /**
         * @return The unexpected status of the HTTP request
         */
        public int Status
        {
            get;
        }

    }
}

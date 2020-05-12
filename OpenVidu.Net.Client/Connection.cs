using System.Collections.Generic;

namespace OpenVidu.Net.Client
{
    public class Connection
    {
        public Connection(string connectionId, long createdAt, OpenViduRole role, string token, string location,
				string platform, string serverData, string clientData, Dictionary<string, Publisher> publishers,
				List<string> subscribers)
		{
			this.ConnectionId = connectionId;
			this.CreatedAt = createdAt;
			this.Role = role;
			this.Token = token;
			this.Location = location;
			this.Platform = platform;
			this.ServerData = serverData;
			this.ClientData = clientData;
			this.Publishers = new List<Publisher>(publishers.Values); 
			this.Subscribers = subscribers;
		}

		/**
		 * Returns the identifier of the connection. You can call
		 * {@link io.openvidu.java.client.Session#forceDisconnect(string)} passing this
		 * property as parameter
		 */
        
		public string ConnectionId
        { get; }

		/**
		 * Timestamp when this connection was established, in UTC milliseconds (ms since
		 * Jan 1, 1970, 00:00:00 UTC)
		 */
		public long CreatedAt
        { get; }

		/**
		 * Returns the role of the connection
		 */
		public OpenViduRole Role
        { get; }

		/**
		 * Returns the token associated to the connection
		 */
		public string Token
        { get; }

		/**
		 * <a href="https://docs.openvidu.io/en/stable/openvidu-pro/" target="_blank" style="display: inline-block;
		 * background-color: rgb(0, 136, 170); color: white; font-weight: bold; padding:
		 * 0px 5px; margin-right: 5px; border-radius: 3px; font-size: 13px;
		 * line-height:21px; font-family: Montserrat, sans-serif">PRO</a>
		 * 
		 * Returns the geo location of the connection, with the following format:
		 * <code>"CITY, COUNTRY"</code> (<code>"unknown"</code> if it wasn't possible to
		 * locate it)
		 */
		public string Location
        { get; }

		/**
		 * Returns a complete description of the platform used by the participant to
		 * connect to the session
		 */
		public string Platform
        { get; }

		/**
		 * Returns the data associated to the connection on the server-side. This value
		 * is set with {@link io.openvidu.java.client.TokenOptions.Builder#data(string)}
		 * when calling {@link io.openvidu.java.client.Session#generateToken()}
		 */
		public string ServerData
        { get; }

		/**
		 * Returns the data associated to the connection on the client-side. This value
		 * is set with second parameter of method <a href=
		 * "https://openvidu.io/api/openvidu-browser/public classes/session.html#connect"
		 * target="_blank">Session.connect</a> in OpenVidu Browser
		 */
		public string ClientData
        { get; }

		/**
		 * Returns the list of PUBLISHER objects this particular Connection is
		 * publishing to the Session (each PUBLISHER object has one Stream, uniquely
		 * identified by its <code>streamId</code>). You can call
		 * {@link io.openvidu.java.client.Session#forceUnpublish(PUBLISHER)} passing any
		 * of this values as parameter
		 */
		public List<Publisher> Publishers
        { get; }

		/**
		 * Returns the list of streams (their <code>streamId</code> properties) this
		 * particular Connection is subscribed to. Each one always corresponds to one
		 * PUBLISHER of some other Connection: each string of the returned list must be
		 * equal to the returned value of some
		 * {@link io.openvidu.java.client.PUBLISHER#getStreamId()}
		 */
		public List<string> Subscribers
        { 
            get;
            set; 
        }


	}
}

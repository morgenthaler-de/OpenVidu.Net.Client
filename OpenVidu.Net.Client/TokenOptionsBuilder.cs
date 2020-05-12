using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public class TokenOptionsBuilder
    {
        private string _data = "";
        private OpenViduRole _role = OpenViduRole.PUBLISHER;
        private KurentoOptions _kurentoOptions;

        /**
		 * Builder for {@link io.openvidu.java.client.TokenOptions}
		 */
        public TokenOptions build()
        {
            return new TokenOptions(this._data, this._role, this._kurentoOptions);
        }

		/**
		 * Call this method to set the secure (server-side) data associated to this
		 * token. Every client will receive this data in property
		 * <code>Connection.data</code>. Object <code>Connection</code> can be retrieved
		 * by subscribing to event <code>connectionCreated</code> of Session object in
		 * your clients.
		 * <ul>
		 * <li>If you have provided no data in your clients when calling method
		 * <code>Session.connect(TOKEN, DATA)</code> (<code>DATA</code> not defined),
		 * then <code>Connection.data</code> will only have this
		 * {@link io.openvidu.java.client.TokenOptions.Builder#data(String)}
		 * property.</li>
		 * <li>If you have provided some data when calling
		 * <code>Session.connect(TOKEN, DATA)</code> (<code>DATA</code> defined), then
		 * <code>Connection.data</code> will have the following structure:
		 * <code>&quot;CLIENT_DATA%/%SERVER_DATA&quot;</code>, being
		 * <code>CLIENT_DATA</code> the second parameter passed in OpenVidu Browser in
		 * method <code>Session.connect</code> and <code>SERVER_DATA</code> this
		 * {@link io.openvidu.java.client.TokenOptions.Builder#data(String)}
		 * property.</li>
		 * </ul>
		 */
		public TokenOptionsBuilder data(string data)
		{
			this._data = data;
			return this;
		}

		/**
		 * Call this method to set the role assigned to this token
		 */
		public TokenOptionsBuilder role(OpenViduRole role)
		{
			this._role = role;
			return this;
		}

		/**
		 * Call this method to set a {@link io.openvidu.java.client.KurentoOptions}
		 * object for this token
		 */
		public TokenOptionsBuilder kurentoOptions(KurentoOptions kurentoOptions)
		{
			this._kurentoOptions = kurentoOptions;
			return this;
		}

	}
}

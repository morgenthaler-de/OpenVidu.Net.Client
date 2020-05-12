using System;
using System.Collections.Generic;
using System.Text;

namespace OpenVidu.Net.Client
{
    public class TokenOptions
    {
        private string _data;
        private OpenViduRole _role;
        private KurentoOptions _kurentoOptions;

        public TokenOptions(string data, OpenViduRole role, KurentoOptions kurentoOptions)
        {
            this._data = data;
            this._role = role;
            this._kurentoOptions = kurentoOptions;
        }


        /**
         * Returns the secure (server-side) metadata assigned to this token
         */
        public string getData()
        {
            return this._data;
        }

        /**
         * Returns the role assigned to this token
         */
        public OpenViduRole getRole()
        {
            return this._role;
        }

        /**
         * Returns the Kurento options assigned to this token
         */
        public KurentoOptions getKurentoOptions()
        {
            return this._kurentoOptions;
        }
	}
}

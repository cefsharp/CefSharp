// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class ProxyOptions
    {
        /// <summary>
        /// The IP address for the proxy
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// The port for the proxy
        /// </summary>
        public string Port { get; private set; }

        /// <summary>
        /// The username for authentication
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for authentication
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The list of domains that shouldn't be affected by the proxy, Format: example.com;example2.com
        /// </summary>
        public string BypassList { get; private set; }

        /// <summary>
        /// Checks if username and password is set
        /// </summary>
        /// <returns>Returns true if both username and password is set, otherwise false</returns>
        public bool HasUsernameAndPassword()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        /// <param name="ip">The IP address for the proxy</param>
        /// <param name="port">The port for the proxy</param>
        /// <param name="username">The username required for authentication</param>
        /// <param name="password">The password required for authentication</param>
        /// <param name="bypassList">The list of domains that shouldn't be affected by the proxy, Format: example.com;example2.com</param>
        public ProxyOptions(string ip, string port, string username = "", string password = "", string bypassList = "")
        {
            IP = ip;
            Port = port;
            Username = username;
            Password = password;
            BypassList = bypassList;
        }
    }
}

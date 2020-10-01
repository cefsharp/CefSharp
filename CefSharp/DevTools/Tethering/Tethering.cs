// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Tethering
{
    using System.Linq;

    /// <summary>
    /// The Tethering domain defines methods and events for browser port binding.
    /// </summary>
    public partial class Tethering : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Tethering(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateBind(int port);
        /// <summary>
        /// Request browser port binding.
        /// </summary>
        /// <param name = "port">Port number to bind.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> BindAsync(int port)
        {
            ValidateBind(port);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("port", port);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tethering.bind", dict);
            return methodResult;
        }

        partial void ValidateUnbind(int port);
        /// <summary>
        /// Request browser port unbinding.
        /// </summary>
        /// <param name = "port">Port number to unbind.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UnbindAsync(int port)
        {
            ValidateUnbind(port);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("port", port);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tethering.unbind", dict);
            return methodResult;
        }
    }
}
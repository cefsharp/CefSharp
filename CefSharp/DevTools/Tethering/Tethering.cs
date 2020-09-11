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
        public Tethering(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Request browser port binding.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> BindAsync(int port)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("port", port);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tethering.bind", dict);
            return methodResult;
        }

        /// <summary>
        /// Request browser port unbinding.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UnbindAsync(int port)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("port", port);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tethering.unbind", dict);
            return methodResult;
        }
    }
}
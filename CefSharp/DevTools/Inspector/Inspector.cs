// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Inspector
{
    using System.Linq;

    /// <summary>
    /// Inspector
    /// </summary>
    public partial class Inspector : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Inspector(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables inspector domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Inspector.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables inspector domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Inspector.enable", dict);
            return methodResult;
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Console
{
    using System.Linq;

    /// <summary>
    /// This domain is deprecated - use Runtime or Log instead.
    /// </summary>
    public partial class Console : DevToolsDomainBase
    {
        public Console(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Does nothing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearMessagesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.clearMessages", dict);
            return result;
        }

        /// <summary>
        /// Disables console domain, prevents further console messages from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables console domain, sends the messages collected so far to the client by means of the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.enable", dict);
            return result;
        }
    }
}
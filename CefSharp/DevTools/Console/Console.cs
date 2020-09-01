// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Console
{
    /// <summary>
    /// This domain is deprecated - use Runtime or Log instead.
    /// </summary>
    public partial class Console
    {
        public Console(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Does nothing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearMessages()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.ClearMessages", dict);
            return result;
        }

        /// <summary>
        /// Disables console domain, prevents further console messages from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enables console domain, sends the messages collected so far to the client by means of the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Console.Enable", dict);
            return result;
        }
    }
}
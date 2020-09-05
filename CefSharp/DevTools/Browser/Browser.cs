// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    using System.Linq;

    /// <summary>
    /// The Browser domain defines methods and events for browser managing.
    /// </summary>
    public partial class Browser : DevToolsDomainBase
    {
        public Browser(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Close browser gracefully.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CloseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.close", dict);
            return result;
        }

        /// <summary>
        /// Returns version information.
        /// </summary>
        public async System.Threading.Tasks.Task<GetVersionResponse> GetVersionAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.getVersion", dict);
            return result.DeserializeJson<GetVersionResponse>();
        }
    }
}
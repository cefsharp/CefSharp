// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Media
{
    using System.Linq;

    /// <summary>
    /// This domain allows detailed inspection of media elements
    /// </summary>
    public partial class Media : DevToolsDomainBase
    {
        public Media(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Enables the Media domain
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Media.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables the Media domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Media.disable", dict);
            return methodResult;
        }
    }
}
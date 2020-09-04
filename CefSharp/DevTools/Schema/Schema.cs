// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Schema
{
    /// <summary>
    /// This domain is deprecated.
    /// </summary>
    public partial class Schema : DevToolsDomainBase
    {
        public Schema(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Returns supported domains.
        /// </summary>
        public async System.Threading.Tasks.Task<GetDomainsResponse> GetDomainsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Schema.getDomains", dict);
            return result.DeserializeJson<GetDomainsResponse>();
        }
    }
}
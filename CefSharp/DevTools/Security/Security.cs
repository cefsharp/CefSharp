// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    using System.Linq;

    /// <summary>
    /// Security
    /// </summary>
    public partial class Security : DevToolsDomainBase
    {
        public Security(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Disables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enable/disable whether all certificate errors should be ignored.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetIgnoreCertificateErrorsAsync(bool ignore)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("ignore", ignore);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.setIgnoreCertificateErrors", dict);
            return methodResult;
        }
    }
}
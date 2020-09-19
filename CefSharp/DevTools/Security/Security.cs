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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Security(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables tracking security state changes.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables tracking security state changes.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.enable", dict);
            return methodResult;
        }

        partial void ValidateSetIgnoreCertificateErrors(bool ignore);
        /// <summary>
        /// Enable/disable whether all certificate errors should be ignored.
        /// </summary>
        /// <param name = "ignore">If true, all certificate errors will be ignored.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetIgnoreCertificateErrorsAsync(bool ignore)
        {
            ValidateSetIgnoreCertificateErrors(ignore);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("ignore", ignore);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Security.setIgnoreCertificateErrors", dict);
            return methodResult;
        }
    }
}
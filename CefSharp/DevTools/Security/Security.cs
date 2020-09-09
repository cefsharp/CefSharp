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
        public Security(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Security.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Security.enable", dict);
            return result;
        }

        /// <summary>
        /// Handles a certificate error that fired a certificateError event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HandleCertificateErrorAsync(int eventId, CertificateErrorAction action)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventId", eventId);
            dict.Add("action", this.EnumToString(action));
            var result = await _client.ExecuteDevToolsMethodAsync("Security.handleCertificateError", dict);
            return result;
        }

        /// <summary>
        /// Enable/disable overriding certificate errors. If enabled, all certificate error events need to
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetOverrideCertificateErrorsAsync(bool @override)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("@override", @override);
            var result = await _client.ExecuteDevToolsMethodAsync("Security.setOverrideCertificateErrors", dict);
            return result;
        }
    }
}
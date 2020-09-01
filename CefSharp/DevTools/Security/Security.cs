// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Security
    /// </summary>
    public partial class Security
    {
        public Security(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Security.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enables tracking security state changes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Security.Enable", dict);
            return result;
        }

        /// <summary>
        /// Enable/disable whether all certificate errors should be ignored.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetIgnoreCertificateErrors(bool ignore)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"ignore", ignore}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Security.SetIgnoreCertificateErrors", dict);
            return result;
        }

        /// <summary>
        /// Handles a certificate error that fired a certificateError event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HandleCertificateError(int eventId, string action)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"eventId", eventId}, {"action", action}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Security.HandleCertificateError", dict);
            return result;
        }

        /// <summary>
        /// Enable/disable overriding certificate errors. If enabled, all certificate error events need to
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetOverrideCertificateErrors(bool @override)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"@override", @override}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Security.SetOverrideCertificateErrors", dict);
            return result;
        }
    }
}
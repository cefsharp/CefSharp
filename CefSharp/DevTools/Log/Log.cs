// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Provides access to log entries.
    /// </summary>
    public partial class Log : DevToolsDomainBase
    {
        public Log(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Clears the log.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.clear", dict);
            return result;
        }

        /// <summary>
        /// Disables log domain, prevents further log entries from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables log domain, sends the entries collected so far to the client by means of the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.enable", dict);
            return result;
        }

        /// <summary>
        /// start violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartViolationsReportAsync(System.Collections.Generic.IList<ViolationSetting> config)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("config", config);
            var result = await _client.ExecuteDevToolsMethodAsync("Log.startViolationsReport", dict);
            return result;
        }

        /// <summary>
        /// Stop violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopViolationsReportAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.stopViolationsReport", dict);
            return result;
        }
    }
}
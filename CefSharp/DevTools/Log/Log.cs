// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    using System.Linq;

    /// <summary>
    /// Provides access to log entries.
    /// </summary>
    public partial class Log : DevToolsDomainBase
    {
        public Log(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Clears the log.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.clear", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables log domain, prevents further log entries from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables log domain, sends the entries collected so far to the client by means of the
        /// `entryAdded` notification.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// start violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartViolationsReportAsync(System.Collections.Generic.IList<CefSharp.DevTools.Log.ViolationSetting> config)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("config", config.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.startViolationsReport", dict);
            return methodResult;
        }

        /// <summary>
        /// Stop violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopViolationsReportAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.stopViolationsReport", dict);
            return methodResult;
        }
    }
}
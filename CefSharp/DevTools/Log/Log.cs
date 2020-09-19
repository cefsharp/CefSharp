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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Log(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.clear", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables log domain, prevents further log entries from being reported to the client.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.enable", dict);
            return methodResult;
        }

        partial void ValidateStartViolationsReport(System.Collections.Generic.IList<CefSharp.DevTools.Log.ViolationSetting> config);
        /// <summary>
        /// start violation reporting.
        /// </summary>
        /// <param name = "config">Configuration for violations.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartViolationsReportAsync(System.Collections.Generic.IList<CefSharp.DevTools.Log.ViolationSetting> config)
        {
            ValidateStartViolationsReport(config);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("config", config.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.startViolationsReport", dict);
            return methodResult;
        }

        /// <summary>
        /// Stop violation reporting.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopViolationsReportAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Log.stopViolationsReport", dict);
            return methodResult;
        }
    }
}
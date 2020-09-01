// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Provides access to log entries.
    /// </summary>
    public partial class Log
    {
        public Log(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Clears the log.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Clear()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.Clear", dict);
            return result;
        }

        /// <summary>
        /// Disables log domain, prevents further log entries from being reported to the client.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enables log domain, sends the entries collected so far to the client by means of the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.Enable", dict);
            return result;
        }

        /// <summary>
        /// start violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartViolationsReport(System.Collections.Generic.IList<ViolationSetting> config)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"config", config}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Log.StartViolationsReport", dict);
            return result;
        }

        /// <summary>
        /// Stop violation reporting.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopViolationsReport()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Log.StopViolationsReport", dict);
            return result;
        }
    }
}
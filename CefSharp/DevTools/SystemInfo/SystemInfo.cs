// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    using System.Linq;

    /// <summary>
    /// The SystemInfo domain defines methods and events for querying low-level system information.
    /// </summary>
    public partial class SystemInfo : DevToolsDomainBase
    {
        public SystemInfo(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Returns information about the system.
        /// </summary>
        public async System.Threading.Tasks.Task<GetInfoResponse> GetInfoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("SystemInfo.getInfo", dict);
            return methodResult.DeserializeJson<GetInfoResponse>();
        }

        /// <summary>
        /// Returns information about all running processes.
        /// </summary>
        public async System.Threading.Tasks.Task<GetProcessInfoResponse> GetProcessInfoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("SystemInfo.getProcessInfo", dict);
            return methodResult.DeserializeJson<GetProcessInfoResponse>();
        }
    }
}
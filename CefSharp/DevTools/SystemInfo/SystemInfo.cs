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
        private CefSharp.DevTools.IDevToolsClient _client;
        public SystemInfo(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Returns information about the system.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetInfoResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetInfoResponse> GetInfoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("SystemInfo.getInfo", dict);
            return methodResult.DeserializeJson<GetInfoResponse>();
        }

        /// <summary>
        /// Returns information about all running processes.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetProcessInfoResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetProcessInfoResponse> GetProcessInfoAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("SystemInfo.getProcessInfo", dict);
            return methodResult.DeserializeJson<GetProcessInfoResponse>();
        }
    }
}
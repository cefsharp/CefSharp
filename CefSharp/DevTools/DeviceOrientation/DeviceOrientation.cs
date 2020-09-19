// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DeviceOrientation
{
    using System.Linq;

    /// <summary>
    /// DeviceOrientation
    /// </summary>
    public partial class DeviceOrientation : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public DeviceOrientation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Clears the overridden Device Orientation.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDeviceOrientationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DeviceOrientation.clearDeviceOrientationOverride", dict);
            return methodResult;
        }

        partial void ValidateSetDeviceOrientationOverride(long alpha, long beta, long gamma);
        /// <summary>
        /// Overrides the Device Orientation.
        /// </summary>
        /// <param name = "alpha">Mock alpha</param>
        /// <param name = "beta">Mock beta</param>
        /// <param name = "gamma">Mock gamma</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDeviceOrientationOverrideAsync(long alpha, long beta, long gamma)
        {
            ValidateSetDeviceOrientationOverride(alpha, beta, gamma);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("alpha", alpha);
            dict.Add("beta", beta);
            dict.Add("gamma", gamma);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DeviceOrientation.setDeviceOrientationOverride", dict);
            return methodResult;
        }
    }
}
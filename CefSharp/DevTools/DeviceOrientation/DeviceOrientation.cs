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
        public DeviceOrientation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Clears the overridden Device Orientation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDeviceOrientationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DeviceOrientation.clearDeviceOrientationOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Overrides the Device Orientation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDeviceOrientationOverrideAsync(long alpha, long beta, long gamma)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("alpha", alpha);
            dict.Add("beta", beta);
            dict.Add("gamma", gamma);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DeviceOrientation.setDeviceOrientationOverride", dict);
            return methodResult;
        }
    }
}
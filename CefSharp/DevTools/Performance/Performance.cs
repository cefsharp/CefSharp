// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Performance
{
    using System.Linq;

    /// <summary>
    /// Performance
    /// </summary>
    public partial class Performance : DevToolsDomainBase
    {
        public Performance(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Disable collecting and reporting metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Performance.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enable collecting and reporting metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(string timeDomain = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(timeDomain)))
            {
                dict.Add("timeDomain", timeDomain);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Performance.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Retrieve current values of run-time metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<GetMetricsResponse> GetMetricsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Performance.getMetrics", dict);
            return methodResult.DeserializeJson<GetMetricsResponse>();
        }
    }
}
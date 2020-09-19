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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Performance(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disable collecting and reporting metrics.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Performance.disable", dict);
            return methodResult;
        }

        partial void ValidateEnable(string timeDomain = null);
        /// <summary>
        /// Enable collecting and reporting metrics.
        /// </summary>
        /// <param name = "timeDomain">Time domain to use for collecting and reporting duration metrics.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(string timeDomain = null)
        {
            ValidateEnable(timeDomain);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;GetMetricsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetMetricsResponse> GetMetricsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Performance.getMetrics", dict);
            return methodResult.DeserializeJson<GetMetricsResponse>();
        }
    }
}
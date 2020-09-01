// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Performance
{
    /// <summary>
    /// Performance
    /// </summary>
    public partial class Performance
    {
        public Performance(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Disable collecting and reporting metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Performance.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enable collecting and reporting metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable(string timeDomain)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"timeDomain", timeDomain}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Performance.Enable", dict);
            return result;
        }

        /// <summary>
        /// Sets time domain to use for collecting and reporting duration metrics.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetTimeDomain(string timeDomain)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"timeDomain", timeDomain}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Performance.SetTimeDomain", dict);
            return result;
        }

        /// <summary>
        /// Retrieve current values of run-time metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetMetrics()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Performance.GetMetrics", dict);
            return result;
        }
    }
}
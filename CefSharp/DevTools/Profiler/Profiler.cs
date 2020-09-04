// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profiler
    /// </summary>
    public partial class Profiler : DevToolsDomainBase
    {
        public Profiler(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.disable", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.enable", dict);
            return result;
        }

        /// <summary>
        /// Collect coverage data for the current isolate. The coverage data may be incomplete due to
        public async System.Threading.Tasks.Task<GetBestEffortCoverageResponse> GetBestEffortCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.getBestEffortCoverage", dict);
            return result.DeserializeJson<GetBestEffortCoverageResponse>();
        }

        /// <summary>
        /// Changes CPU profiler sampling interval. Must be called before CPU profiles recording started.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetSamplingIntervalAsync(int interval)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interval", interval);
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.setSamplingInterval", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.start", dict);
            return result;
        }

        /// <summary>
        /// Enable precise code coverage. Coverage data for JavaScript executed before enabling precise code
        public async System.Threading.Tasks.Task<StartPreciseCoverageResponse> StartPreciseCoverageAsync(bool? callCount = null, bool? detailed = null, bool? allowTriggeredUpdates = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (callCount.HasValue)
            {
                dict.Add("callCount", callCount.Value);
            }

            if (detailed.HasValue)
            {
                dict.Add("detailed", detailed.Value);
            }

            if (allowTriggeredUpdates.HasValue)
            {
                dict.Add("allowTriggeredUpdates", allowTriggeredUpdates.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.startPreciseCoverage", dict);
            return result.DeserializeJson<StartPreciseCoverageResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<StopResponse> StopAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.stop", dict);
            return result.DeserializeJson<StopResponse>();
        }

        /// <summary>
        /// Disable precise code coverage. Disabling releases unnecessary execution count records and allows
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopPreciseCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.stopPreciseCoverage", dict);
            return result;
        }

        /// <summary>
        /// Collect coverage data for the current isolate, and resets execution counters. Precise code
        public async System.Threading.Tasks.Task<TakePreciseCoverageResponse> TakePreciseCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.takePreciseCoverage", dict);
            return result.DeserializeJson<TakePreciseCoverageResponse>();
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    using System.Linq;

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
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Collect coverage data for the current isolate. The coverage data may be incomplete due to
        public async System.Threading.Tasks.Task<GetBestEffortCoverageResponse> GetBestEffortCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.getBestEffortCoverage", dict);
            return methodResult.DeserializeJson<GetBestEffortCoverageResponse>();
        }

        /// <summary>
        /// Changes CPU profiler sampling interval. Must be called before CPU profiles recording started.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetSamplingIntervalAsync(int interval)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("interval", interval);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.setSamplingInterval", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.start", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.startPreciseCoverage", dict);
            return methodResult.DeserializeJson<StartPreciseCoverageResponse>();
        }

        /// <summary>
        /// Enable type profile.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartTypeProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.startTypeProfile", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<StopResponse> StopAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.stop", dict);
            return methodResult.DeserializeJson<StopResponse>();
        }

        /// <summary>
        /// Disable precise code coverage. Disabling releases unnecessary execution count records and allows
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopPreciseCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.stopPreciseCoverage", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable type profile. Disabling releases type profile data collected so far.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopTypeProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.stopTypeProfile", dict);
            return methodResult;
        }

        /// <summary>
        /// Collect coverage data for the current isolate, and resets execution counters. Precise code
        public async System.Threading.Tasks.Task<TakePreciseCoverageResponse> TakePreciseCoverageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.takePreciseCoverage", dict);
            return methodResult.DeserializeJson<TakePreciseCoverageResponse>();
        }

        /// <summary>
        /// Collect type profile.
        /// </summary>
        public async System.Threading.Tasks.Task<TakeTypeProfileResponse> TakeTypeProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.takeTypeProfile", dict);
            return methodResult.DeserializeJson<TakeTypeProfileResponse>();
        }

        /// <summary>
        /// Enable run time call stats collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableRuntimeCallStatsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.enableRuntimeCallStats", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable run time call stats collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableRuntimeCallStatsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.disableRuntimeCallStats", dict);
            return methodResult;
        }

        /// <summary>
        /// Retrieve run time call stats.
        /// </summary>
        public async System.Threading.Tasks.Task<GetRuntimeCallStatsResponse> GetRuntimeCallStatsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Profiler.getRuntimeCallStats", dict);
            return methodResult.DeserializeJson<GetRuntimeCallStatsResponse>();
        }
    }
}
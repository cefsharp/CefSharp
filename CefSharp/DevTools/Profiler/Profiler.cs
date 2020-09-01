// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Profiler
{
    /// <summary>
    /// Profiler
    /// </summary>
    public partial class Profiler
    {
        public Profiler(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.Disable", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.Enable", dict);
            return result;
        }

        /// <summary>
        /// Collect coverage data for the current isolate. The coverage data may be incomplete due to
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetBestEffortCoverage()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.GetBestEffortCoverage", dict);
            return result;
        }

        /// <summary>
        /// Changes CPU profiler sampling interval. Must be called before CPU profiles recording started.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetSamplingInterval(int interval)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"interval", interval}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.SetSamplingInterval", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Start()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.Start", dict);
            return result;
        }

        /// <summary>
        /// Enable precise code coverage. Coverage data for JavaScript executed before enabling precise code
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartPreciseCoverage(bool callCount, bool detailed, bool allowTriggeredUpdates)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"callCount", callCount}, {"detailed", detailed}, {"allowTriggeredUpdates", allowTriggeredUpdates}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.StartPreciseCoverage", dict);
            return result;
        }

        /// <summary>
        /// Enable type profile.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartTypeProfile()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.StartTypeProfile", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Stop()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.Stop", dict);
            return result;
        }

        /// <summary>
        /// Disable precise code coverage. Disabling releases unnecessary execution count records and allows
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopPreciseCoverage()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.StopPreciseCoverage", dict);
            return result;
        }

        /// <summary>
        /// Disable type profile. Disabling releases type profile data collected so far.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopTypeProfile()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.StopTypeProfile", dict);
            return result;
        }

        /// <summary>
        /// Collect coverage data for the current isolate, and resets execution counters. Precise code
        public async System.Threading.Tasks.Task<DevToolsMethodResult> TakePreciseCoverage()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.TakePreciseCoverage", dict);
            return result;
        }

        /// <summary>
        /// Collect type profile.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> TakeTypeProfile()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.TakeTypeProfile", dict);
            return result;
        }

        /// <summary>
        /// Enable run time call stats collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableRuntimeCallStats()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.EnableRuntimeCallStats", dict);
            return result;
        }

        /// <summary>
        /// Disable run time call stats collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableRuntimeCallStats()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.DisableRuntimeCallStats", dict);
            return result;
        }

        /// <summary>
        /// Retrieve run time call stats.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetRuntimeCallStats()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Profiler.GetRuntimeCallStats", dict);
            return result;
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Memory
{
    using System.Linq;

    /// <summary>
    /// Memory
    /// </summary>
    public partial class Memory : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Memory(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// GetDOMCounters
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetDOMCountersResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetDOMCountersResponse> GetDOMCountersAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.getDOMCounters", dict);
            return methodResult.DeserializeJson<GetDOMCountersResponse>();
        }

        /// <summary>
        /// PrepareForLeakDetection
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> PrepareForLeakDetectionAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.prepareForLeakDetection", dict);
            return methodResult;
        }

        /// <summary>
        /// Simulate OomIntervention by purging V8 memory.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ForciblyPurgeJavaScriptMemoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.forciblyPurgeJavaScriptMemory", dict);
            return methodResult;
        }

        partial void ValidateSetPressureNotificationsSuppressed(bool suppressed);
        /// <summary>
        /// Enable/disable suppressing memory pressure notifications in all processes.
        /// </summary>
        /// <param name = "suppressed">If true, memory pressure notifications will be suppressed.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPressureNotificationsSuppressedAsync(bool suppressed)
        {
            ValidateSetPressureNotificationsSuppressed(suppressed);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("suppressed", suppressed);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.setPressureNotificationsSuppressed", dict);
            return methodResult;
        }

        partial void ValidateSimulatePressureNotification(CefSharp.DevTools.Memory.PressureLevel level);
        /// <summary>
        /// Simulate a memory pressure notification in all processes.
        /// </summary>
        /// <param name = "level">Memory pressure level of the notification.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SimulatePressureNotificationAsync(CefSharp.DevTools.Memory.PressureLevel level)
        {
            ValidateSimulatePressureNotification(level);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("level", this.EnumToString(level));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.simulatePressureNotification", dict);
            return methodResult;
        }

        partial void ValidateStartSampling(int? samplingInterval = null, bool? suppressRandomness = null);
        /// <summary>
        /// Start collecting native memory profile.
        /// </summary>
        /// <param name = "samplingInterval">Average number of bytes between samples.</param>
        /// <param name = "suppressRandomness">Do not randomize intervals between samples.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartSamplingAsync(int? samplingInterval = null, bool? suppressRandomness = null)
        {
            ValidateStartSampling(samplingInterval, suppressRandomness);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (samplingInterval.HasValue)
            {
                dict.Add("samplingInterval", samplingInterval.Value);
            }

            if (suppressRandomness.HasValue)
            {
                dict.Add("suppressRandomness", suppressRandomness.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.startSampling", dict);
            return methodResult;
        }

        /// <summary>
        /// Stop collecting native memory profile.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopSamplingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.stopSampling", dict);
            return methodResult;
        }

        /// <summary>
        /// Retrieve native memory allocations profile
        /// collected since renderer process startup.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetAllTimeSamplingProfileResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetAllTimeSamplingProfileResponse> GetAllTimeSamplingProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.getAllTimeSamplingProfile", dict);
            return methodResult.DeserializeJson<GetAllTimeSamplingProfileResponse>();
        }

        /// <summary>
        /// Retrieve native memory allocations profile
        /// collected since browser process startup.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetBrowserSamplingProfileResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetBrowserSamplingProfileResponse> GetBrowserSamplingProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.getBrowserSamplingProfile", dict);
            return methodResult.DeserializeJson<GetBrowserSamplingProfileResponse>();
        }

        /// <summary>
        /// Retrieve native memory allocations profile collected since last
        /// `startSampling` call.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetSamplingProfileResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetSamplingProfileResponse> GetSamplingProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Memory.getSamplingProfile", dict);
            return methodResult.DeserializeJson<GetSamplingProfileResponse>();
        }
    }
}
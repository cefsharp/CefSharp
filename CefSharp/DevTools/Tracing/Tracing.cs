// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Tracing
{
    using System.Linq;

    /// <summary>
    /// Tracing
    /// </summary>
    public partial class Tracing : DevToolsDomainBase
    {
        public Tracing(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Stop trace events collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EndAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.end", dict);
            return methodResult;
        }

        /// <summary>
        /// Gets supported tracing categories.
        /// </summary>
        public async System.Threading.Tasks.Task<GetCategoriesResponse> GetCategoriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.getCategories", dict);
            return methodResult.DeserializeJson<GetCategoriesResponse>();
        }

        /// <summary>
        /// Record a clock sync marker in the trace.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RecordClockSyncMarkerAsync(string syncId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("syncId", syncId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.recordClockSyncMarker", dict);
            return methodResult;
        }

        /// <summary>
        /// Request a global memory dump.
        /// </summary>
        public async System.Threading.Tasks.Task<RequestMemoryDumpResponse> RequestMemoryDumpAsync(bool? deterministic = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (deterministic.HasValue)
            {
                dict.Add("deterministic", deterministic.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.requestMemoryDump", dict);
            return methodResult.DeserializeJson<RequestMemoryDumpResponse>();
        }

        /// <summary>
        /// Start trace events collection.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartAsync(string categories = null, string options = null, long? bufferUsageReportingInterval = null, string transferMode = null, CefSharp.DevTools.Tracing.StreamFormat? streamFormat = null, CefSharp.DevTools.Tracing.StreamCompression? streamCompression = null, CefSharp.DevTools.Tracing.TraceConfig traceConfig = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(categories)))
            {
                dict.Add("categories", categories);
            }

            if (!(string.IsNullOrEmpty(options)))
            {
                dict.Add("options", options);
            }

            if (bufferUsageReportingInterval.HasValue)
            {
                dict.Add("bufferUsageReportingInterval", bufferUsageReportingInterval.Value);
            }

            if (!(string.IsNullOrEmpty(transferMode)))
            {
                dict.Add("transferMode", transferMode);
            }

            if (streamFormat.HasValue)
            {
                dict.Add("streamFormat", this.EnumToString(streamFormat));
            }

            if (streamCompression.HasValue)
            {
                dict.Add("streamCompression", this.EnumToString(streamCompression));
            }

            if ((traceConfig) != (null))
            {
                dict.Add("traceConfig", traceConfig.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.start", dict);
            return methodResult;
        }
    }
}
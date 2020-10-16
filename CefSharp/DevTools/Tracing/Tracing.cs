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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Tracing(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Stop trace events collection.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EndAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.end", dict);
            return methodResult;
        }

        /// <summary>
        /// Gets supported tracing categories.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCategoriesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCategoriesResponse> GetCategoriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.getCategories", dict);
            return methodResult.DeserializeJson<GetCategoriesResponse>();
        }

        partial void ValidateRecordClockSyncMarker(string syncId);
        /// <summary>
        /// Record a clock sync marker in the trace.
        /// </summary>
        /// <param name = "syncId">The ID of this clock sync marker</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RecordClockSyncMarkerAsync(string syncId)
        {
            ValidateRecordClockSyncMarker(syncId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("syncId", syncId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.recordClockSyncMarker", dict);
            return methodResult;
        }

        partial void ValidateRequestMemoryDump(bool? deterministic = null);
        /// <summary>
        /// Request a global memory dump.
        /// </summary>
        /// <param name = "deterministic">Enables more deterministic results by forcing garbage collection</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RequestMemoryDumpResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RequestMemoryDumpResponse> RequestMemoryDumpAsync(bool? deterministic = null)
        {
            ValidateRequestMemoryDump(deterministic);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (deterministic.HasValue)
            {
                dict.Add("deterministic", deterministic.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Tracing.requestMemoryDump", dict);
            return methodResult.DeserializeJson<RequestMemoryDumpResponse>();
        }

        partial void ValidateStart(string categories = null, string options = null, long? bufferUsageReportingInterval = null, string transferMode = null, CefSharp.DevTools.Tracing.StreamFormat? streamFormat = null, CefSharp.DevTools.Tracing.StreamCompression? streamCompression = null, CefSharp.DevTools.Tracing.TraceConfig traceConfig = null);
        /// <summary>
        /// Start trace events collection.
        /// </summary>
        /// <param name = "categories">Category/tag filter</param>
        /// <param name = "options">Tracing options</param>
        /// <param name = "bufferUsageReportingInterval">If set, the agent will issue bufferUsage events at this interval, specified in milliseconds</param>
        /// <param name = "transferMode">Whether to report trace events as series of dataCollected events or to save trace to astream (defaults to `ReportEvents`).</param>
        /// <param name = "streamFormat">Trace data format to use. This only applies when using `ReturnAsStream`transfer mode (defaults to `json`).</param>
        /// <param name = "streamCompression">Compression format to use. This only applies when using `ReturnAsStream`transfer mode (defaults to `none`)</param>
        /// <param name = "traceConfig">traceConfig</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartAsync(string categories = null, string options = null, long? bufferUsageReportingInterval = null, string transferMode = null, CefSharp.DevTools.Tracing.StreamFormat? streamFormat = null, CefSharp.DevTools.Tracing.StreamCompression? streamCompression = null, CefSharp.DevTools.Tracing.TraceConfig traceConfig = null)
        {
            ValidateStart(categories, options, bufferUsageReportingInterval, transferMode, streamFormat, streamCompression, traceConfig);
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
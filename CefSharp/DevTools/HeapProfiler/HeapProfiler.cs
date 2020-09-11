// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    using System.Linq;

    /// <summary>
    /// HeapProfiler
    /// </summary>
    public partial class HeapProfiler : DevToolsDomainBase
    {
        public HeapProfiler(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddInspectedHeapObjectAsync(string heapObjectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("heapObjectId", heapObjectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.addInspectedHeapObject", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CollectGarbageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.collectGarbage", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetHeapObjectIdResponse> GetHeapObjectIdAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.getHeapObjectId", dict);
            return methodResult.DeserializeJson<GetHeapObjectIdResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetObjectByHeapObjectIdResponse> GetObjectByHeapObjectIdAsync(string objectId, string objectGroup = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.getObjectByHeapObjectId", dict);
            return methodResult.DeserializeJson<GetObjectByHeapObjectIdResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetSamplingProfileResponse> GetSamplingProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.getSamplingProfile", dict);
            return methodResult.DeserializeJson<GetSamplingProfileResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartSamplingAsync(long? samplingInterval = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (samplingInterval.HasValue)
            {
                dict.Add("samplingInterval", samplingInterval.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.startSampling", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartTrackingHeapObjectsAsync(bool? trackAllocations = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (trackAllocations.HasValue)
            {
                dict.Add("trackAllocations", trackAllocations.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.startTrackingHeapObjects", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<StopSamplingResponse> StopSamplingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.stopSampling", dict);
            return methodResult.DeserializeJson<StopSamplingResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopTrackingHeapObjectsAsync(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (reportProgress.HasValue)
            {
                dict.Add("reportProgress", reportProgress.Value);
            }

            if (treatGlobalObjectsAsRoots.HasValue)
            {
                dict.Add("treatGlobalObjectsAsRoots", treatGlobalObjectsAsRoots.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.stopTrackingHeapObjects", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TakeHeapSnapshotAsync(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (reportProgress.HasValue)
            {
                dict.Add("reportProgress", reportProgress.Value);
            }

            if (treatGlobalObjectsAsRoots.HasValue)
            {
                dict.Add("treatGlobalObjectsAsRoots", treatGlobalObjectsAsRoots.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.takeHeapSnapshot", dict);
            return methodResult;
        }
    }
}
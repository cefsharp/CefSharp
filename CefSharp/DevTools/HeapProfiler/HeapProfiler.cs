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
        private CefSharp.DevTools.IDevToolsClient _client;
        public HeapProfiler(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateAddInspectedHeapObject(string heapObjectId);
        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        /// $x functions).
        /// </summary>
        /// <param name = "heapObjectId">Heap snapshot object id to be accessible by means of $x command line API.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddInspectedHeapObjectAsync(string heapObjectId)
        {
            ValidateAddInspectedHeapObject(heapObjectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("heapObjectId", heapObjectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.addInspectedHeapObject", dict);
            return methodResult;
        }

        /// <summary>
        /// CollectGarbage
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CollectGarbageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.collectGarbage", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enable
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.enable", dict);
            return methodResult;
        }

        partial void ValidateGetHeapObjectId(string objectId);
        /// <summary>
        /// GetHeapObjectId
        /// </summary>
        /// <param name = "objectId">Identifier of the object to get heap object id for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetHeapObjectIdResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetHeapObjectIdResponse> GetHeapObjectIdAsync(string objectId)
        {
            ValidateGetHeapObjectId(objectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.getHeapObjectId", dict);
            return methodResult.DeserializeJson<GetHeapObjectIdResponse>();
        }

        partial void ValidateGetObjectByHeapObjectId(string objectId, string objectGroup = null);
        /// <summary>
        /// GetObjectByHeapObjectId
        /// </summary>
        /// <param name = "objectId">objectId</param>
        /// <param name = "objectGroup">Symbolic group name that can be used to release multiple objects.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetObjectByHeapObjectIdResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetObjectByHeapObjectIdResponse> GetObjectByHeapObjectIdAsync(string objectId, string objectGroup = null)
        {
            ValidateGetObjectByHeapObjectId(objectId, objectGroup);
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
        /// GetSamplingProfile
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetSamplingProfileResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetSamplingProfileResponse> GetSamplingProfileAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.getSamplingProfile", dict);
            return methodResult.DeserializeJson<GetSamplingProfileResponse>();
        }

        partial void ValidateStartSampling(long? samplingInterval = null);
        /// <summary>
        /// StartSampling
        /// </summary>
        /// <param name = "samplingInterval">Average sample interval in bytes. Poisson distribution is used for the intervals. The
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartSamplingAsync(long? samplingInterval = null)
        {
            ValidateStartSampling(samplingInterval);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (samplingInterval.HasValue)
            {
                dict.Add("samplingInterval", samplingInterval.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.startSampling", dict);
            return methodResult;
        }

        partial void ValidateStartTrackingHeapObjects(bool? trackAllocations = null);
        /// <summary>
        /// StartTrackingHeapObjects
        /// </summary>
        /// <param name = "trackAllocations">trackAllocations</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartTrackingHeapObjectsAsync(bool? trackAllocations = null)
        {
            ValidateStartTrackingHeapObjects(trackAllocations);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (trackAllocations.HasValue)
            {
                dict.Add("trackAllocations", trackAllocations.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.startTrackingHeapObjects", dict);
            return methodResult;
        }

        /// <summary>
        /// StopSampling
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;StopSamplingResponse&gt;</returns>
        public async System.Threading.Tasks.Task<StopSamplingResponse> StopSamplingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeapProfiler.stopSampling", dict);
            return methodResult.DeserializeJson<StopSamplingResponse>();
        }

        partial void ValidateStopTrackingHeapObjects(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null);
        /// <summary>
        /// StopTrackingHeapObjects
        /// </summary>
        /// <param name = "reportProgress">If true 'reportHeapSnapshotProgress' events will be generated while snapshot is being taken
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopTrackingHeapObjectsAsync(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null)
        {
            ValidateStopTrackingHeapObjects(reportProgress, treatGlobalObjectsAsRoots);
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

        partial void ValidateTakeHeapSnapshot(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null);
        /// <summary>
        /// TakeHeapSnapshot
        /// </summary>
        /// <param name = "reportProgress">If true 'reportHeapSnapshotProgress' events will be generated while snapshot is being taken.</param>
        /// <param name = "treatGlobalObjectsAsRoots">If true, a raw snapshot without artifical roots will be generated</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TakeHeapSnapshotAsync(bool? reportProgress = null, bool? treatGlobalObjectsAsRoots = null)
        {
            ValidateTakeHeapSnapshot(reportProgress, treatGlobalObjectsAsRoots);
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
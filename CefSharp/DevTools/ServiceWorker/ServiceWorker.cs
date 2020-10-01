// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ServiceWorker
{
    using System.Linq;

    /// <summary>
    /// ServiceWorker
    /// </summary>
    public partial class ServiceWorker : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public ServiceWorker(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateDeliverPushMessage(string origin, string registrationId, string data);
        /// <summary>
        /// DeliverPushMessage
        /// </summary>
        /// <param name = "origin">origin</param>
        /// <param name = "registrationId">registrationId</param>
        /// <param name = "data">data</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeliverPushMessageAsync(string origin, string registrationId, string data)
        {
            ValidateDeliverPushMessage(origin, registrationId, data);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            dict.Add("registrationId", registrationId);
            dict.Add("data", data);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.deliverPushMessage", dict);
            return methodResult;
        }

        /// <summary>
        /// Disable
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.disable", dict);
            return methodResult;
        }

        partial void ValidateDispatchSyncEvent(string origin, string registrationId, string tag, bool lastChance);
        /// <summary>
        /// DispatchSyncEvent
        /// </summary>
        /// <param name = "origin">origin</param>
        /// <param name = "registrationId">registrationId</param>
        /// <param name = "tag">tag</param>
        /// <param name = "lastChance">lastChance</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DispatchSyncEventAsync(string origin, string registrationId, string tag, bool lastChance)
        {
            ValidateDispatchSyncEvent(origin, registrationId, tag, lastChance);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            dict.Add("registrationId", registrationId);
            dict.Add("tag", tag);
            dict.Add("lastChance", lastChance);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.dispatchSyncEvent", dict);
            return methodResult;
        }

        partial void ValidateDispatchPeriodicSyncEvent(string origin, string registrationId, string tag);
        /// <summary>
        /// DispatchPeriodicSyncEvent
        /// </summary>
        /// <param name = "origin">origin</param>
        /// <param name = "registrationId">registrationId</param>
        /// <param name = "tag">tag</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DispatchPeriodicSyncEventAsync(string origin, string registrationId, string tag)
        {
            ValidateDispatchPeriodicSyncEvent(origin, registrationId, tag);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("origin", origin);
            dict.Add("registrationId", registrationId);
            dict.Add("tag", tag);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.dispatchPeriodicSyncEvent", dict);
            return methodResult;
        }

        /// <summary>
        /// Enable
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.enable", dict);
            return methodResult;
        }

        partial void ValidateInspectWorker(string versionId);
        /// <summary>
        /// InspectWorker
        /// </summary>
        /// <param name = "versionId">versionId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> InspectWorkerAsync(string versionId)
        {
            ValidateInspectWorker(versionId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("versionId", versionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.inspectWorker", dict);
            return methodResult;
        }

        partial void ValidateSetForceUpdateOnPageLoad(bool forceUpdateOnPageLoad);
        /// <summary>
        /// SetForceUpdateOnPageLoad
        /// </summary>
        /// <param name = "forceUpdateOnPageLoad">forceUpdateOnPageLoad</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetForceUpdateOnPageLoadAsync(bool forceUpdateOnPageLoad)
        {
            ValidateSetForceUpdateOnPageLoad(forceUpdateOnPageLoad);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("forceUpdateOnPageLoad", forceUpdateOnPageLoad);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.setForceUpdateOnPageLoad", dict);
            return methodResult;
        }

        partial void ValidateSkipWaiting(string scopeURL);
        /// <summary>
        /// SkipWaiting
        /// </summary>
        /// <param name = "scopeURL">scopeURL</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SkipWaitingAsync(string scopeURL)
        {
            ValidateSkipWaiting(scopeURL);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeURL", scopeURL);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.skipWaiting", dict);
            return methodResult;
        }

        partial void ValidateStartWorker(string scopeURL);
        /// <summary>
        /// StartWorker
        /// </summary>
        /// <param name = "scopeURL">scopeURL</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartWorkerAsync(string scopeURL)
        {
            ValidateStartWorker(scopeURL);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeURL", scopeURL);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.startWorker", dict);
            return methodResult;
        }

        /// <summary>
        /// StopAllWorkers
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopAllWorkersAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.stopAllWorkers", dict);
            return methodResult;
        }

        partial void ValidateStopWorker(string versionId);
        /// <summary>
        /// StopWorker
        /// </summary>
        /// <param name = "versionId">versionId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopWorkerAsync(string versionId)
        {
            ValidateStopWorker(versionId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("versionId", versionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.stopWorker", dict);
            return methodResult;
        }

        partial void ValidateUnregister(string scopeURL);
        /// <summary>
        /// Unregister
        /// </summary>
        /// <param name = "scopeURL">scopeURL</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UnregisterAsync(string scopeURL)
        {
            ValidateUnregister(scopeURL);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeURL", scopeURL);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.unregister", dict);
            return methodResult;
        }

        partial void ValidateUpdateRegistration(string scopeURL);
        /// <summary>
        /// UpdateRegistration
        /// </summary>
        /// <param name = "scopeURL">scopeURL</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> UpdateRegistrationAsync(string scopeURL)
        {
            ValidateUpdateRegistration(scopeURL);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeURL", scopeURL);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("ServiceWorker.updateRegistration", dict);
            return methodResult;
        }
    }
}
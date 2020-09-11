// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.BackgroundService
{
    using System.Linq;

    /// <summary>
    /// Defines events for background web platform features.
    /// </summary>
    public partial class BackgroundService : DevToolsDomainBase
    {
        public BackgroundService(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Enables event updates for the service.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartObservingAsync(CefSharp.DevTools.BackgroundService.ServiceName service)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("service", this.EnumToString(service));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("BackgroundService.startObserving", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables event updates for the service.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopObservingAsync(CefSharp.DevTools.BackgroundService.ServiceName service)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("service", this.EnumToString(service));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("BackgroundService.stopObserving", dict);
            return methodResult;
        }

        /// <summary>
        /// Set the recording state for the service.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetRecordingAsync(bool shouldRecord, CefSharp.DevTools.BackgroundService.ServiceName service)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("shouldRecord", shouldRecord);
            dict.Add("service", this.EnumToString(service));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("BackgroundService.setRecording", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears all stored data for the service.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearEventsAsync(CefSharp.DevTools.BackgroundService.ServiceName service)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("service", this.EnumToString(service));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("BackgroundService.clearEvents", dict);
            return methodResult;
        }
    }
}
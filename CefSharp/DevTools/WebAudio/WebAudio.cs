// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    using System.Linq;

    /// <summary>
    /// This domain allows inspection of Web Audio API.
    /// https://webaudio.github.io/web-audio-api/
    /// </summary>
    public partial class WebAudio : DevToolsDomainBase
    {
        public WebAudio(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Enables the WebAudio domain and starts sending context lifetime events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAudio.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables the WebAudio domain.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAudio.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Fetch the realtime data from the registered contexts.
        /// </summary>
        public async System.Threading.Tasks.Task<GetRealtimeDataResponse> GetRealtimeDataAsync(string contextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("contextId", contextId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("WebAudio.getRealtimeData", dict);
            return methodResult.DeserializeJson<GetRealtimeDataResponse>();
        }
    }
}
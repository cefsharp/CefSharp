// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Cast
{
    using System.Linq;

    /// <summary>
    /// A domain for interacting with Cast, Presentation API, and Remote Playback API
    public partial class Cast : DevToolsDomainBase
    {
        public Cast(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Starts observing for sinks that can be used for tab mirroring, and if set,
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(string presentationUrl = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(presentationUrl)))
            {
                dict.Add("presentationUrl", presentationUrl);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Stops observing for sinks and issues.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets a sink to be used when the web page requests the browser to choose a
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetSinkToUseAsync(string sinkName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.setSinkToUse", dict);
            return methodResult;
        }

        /// <summary>
        /// Starts mirroring the tab to the sink.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartTabMirroringAsync(string sinkName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.startTabMirroring", dict);
            return methodResult;
        }

        /// <summary>
        /// Stops the active Cast session on the sink.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopCastingAsync(string sinkName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.stopCasting", dict);
            return methodResult;
        }
    }
}
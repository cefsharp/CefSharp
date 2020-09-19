// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Cast
{
    using System.Linq;

    /// <summary>
    /// A domain for interacting with Cast, Presentation API, and Remote Playback API
    /// functionalities.
    /// </summary>
    public partial class Cast : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Cast(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateEnable(string presentationUrl = null);
        /// <summary>
        /// Starts observing for sinks that can be used for tab mirroring, and if set,
        /// sinks compatible with |presentationUrl| as well. When sinks are found, a
        /// |sinksUpdated| event is fired.
        /// Also starts observing for issue messages. When an issue is added or removed,
        /// an |issueUpdated| event is fired.
        /// </summary>
        /// <param name = "presentationUrl">presentationUrl</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync(string presentationUrl = null)
        {
            ValidateEnable(presentationUrl);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.disable", dict);
            return methodResult;
        }

        partial void ValidateSetSinkToUse(string sinkName);
        /// <summary>
        /// Sets a sink to be used when the web page requests the browser to choose a
        /// sink via Presentation API, Remote Playback API, or Cast SDK.
        /// </summary>
        /// <param name = "sinkName">sinkName</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetSinkToUseAsync(string sinkName)
        {
            ValidateSetSinkToUse(sinkName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.setSinkToUse", dict);
            return methodResult;
        }

        partial void ValidateStartTabMirroring(string sinkName);
        /// <summary>
        /// Starts mirroring the tab to the sink.
        /// </summary>
        /// <param name = "sinkName">sinkName</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartTabMirroringAsync(string sinkName)
        {
            ValidateStartTabMirroring(sinkName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.startTabMirroring", dict);
            return methodResult;
        }

        partial void ValidateStopCasting(string sinkName);
        /// <summary>
        /// Stops the active Cast session on the sink.
        /// </summary>
        /// <param name = "sinkName">sinkName</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopCastingAsync(string sinkName)
        {
            ValidateStopCasting(sinkName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sinkName", sinkName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Cast.stopCasting", dict);
            return methodResult;
        }
    }
}
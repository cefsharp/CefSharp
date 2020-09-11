// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeadlessExperimental
{
    using System.Linq;

    /// <summary>
    /// This domain provides experimental commands only supported in headless mode.
    /// </summary>
    public partial class HeadlessExperimental : DevToolsDomainBase
    {
        public HeadlessExperimental(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Sends a BeginFrame to the target and returns when the frame was completed. Optionally captures a
        public async System.Threading.Tasks.Task<BeginFrameResponse> BeginFrameAsync(long? frameTimeTicks = null, long? interval = null, bool? noDisplayUpdates = null, CefSharp.DevTools.HeadlessExperimental.ScreenshotParams screenshot = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (frameTimeTicks.HasValue)
            {
                dict.Add("frameTimeTicks", frameTimeTicks.Value);
            }

            if (interval.HasValue)
            {
                dict.Add("interval", interval.Value);
            }

            if (noDisplayUpdates.HasValue)
            {
                dict.Add("noDisplayUpdates", noDisplayUpdates.Value);
            }

            if ((screenshot) != (null))
            {
                dict.Add("screenshot", screenshot.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeadlessExperimental.beginFrame", dict);
            return methodResult.DeserializeJson<BeginFrameResponse>();
        }

        /// <summary>
        /// Disables headless events for the target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeadlessExperimental.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables headless events for the target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeadlessExperimental.enable", dict);
            return methodResult;
        }
    }
}
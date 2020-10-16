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
        private CefSharp.DevTools.IDevToolsClient _client;
        public HeadlessExperimental(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateBeginFrame(long? frameTimeTicks = null, long? interval = null, bool? noDisplayUpdates = null, CefSharp.DevTools.HeadlessExperimental.ScreenshotParams screenshot = null);
        /// <summary>
        /// Sends a BeginFrame to the target and returns when the frame was completed. Optionally captures a
        /// screenshot from the resulting frame. Requires that the target was created with enabled
        /// BeginFrameControl. Designed for use with --run-all-compositor-stages-before-draw, see also
        /// https://goo.gl/3zHXhB for more background.
        /// </summary>
        /// <param name = "frameTimeTicks">Timestamp of this BeginFrame in Renderer TimeTicks (milliseconds of uptime). If not set,the current time will be used.</param>
        /// <param name = "interval">The interval between BeginFrames that is reported to the compositor, in milliseconds.Defaults to a 60 frames/second interval, i.e. about 16.666 milliseconds.</param>
        /// <param name = "noDisplayUpdates">Whether updates should not be committed and drawn onto the display. False by default. Iftrue, only side effects of the BeginFrame will be run, such as layout and animations, butany visual updates may not be visible on the display or in screenshots.</param>
        /// <param name = "screenshot">If set, a screenshot of the frame will be captured and returned in the response. Otherwise,no screenshot will be captured. Note that capturing a screenshot can fail, for example,during renderer initialization. In such a case, no screenshot data will be returned.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;BeginFrameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<BeginFrameResponse> BeginFrameAsync(long? frameTimeTicks = null, long? interval = null, bool? noDisplayUpdates = null, CefSharp.DevTools.HeadlessExperimental.ScreenshotParams screenshot = null)
        {
            ValidateBeginFrame(frameTimeTicks, interval, noDisplayUpdates, screenshot);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeadlessExperimental.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables headless events for the target.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("HeadlessExperimental.enable", dict);
            return methodResult;
        }
    }
}
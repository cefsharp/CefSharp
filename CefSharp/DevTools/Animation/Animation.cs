// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Animation
{
    using System.Linq;

    /// <summary>
    /// Animation
    /// </summary>
    public partial class Animation : DevToolsDomainBase
    {
        public Animation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Disables animation domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables animation domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns the current time of the an animation.
        /// </summary>
        public async System.Threading.Tasks.Task<GetCurrentTimeResponse> GetCurrentTimeAsync(string id)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("id", id);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.getCurrentTime", dict);
            return methodResult.DeserializeJson<GetCurrentTimeResponse>();
        }

        /// <summary>
        /// Gets the playback rate of the document timeline.
        /// </summary>
        public async System.Threading.Tasks.Task<GetPlaybackRateResponse> GetPlaybackRateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.getPlaybackRate", dict);
            return methodResult.DeserializeJson<GetPlaybackRateResponse>();
        }

        /// <summary>
        /// Releases a set of animations to no longer be manipulated.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseAnimationsAsync(string[] animations)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.releaseAnimations", dict);
            return methodResult;
        }

        /// <summary>
        /// Gets the remote object of the Animation.
        /// </summary>
        public async System.Threading.Tasks.Task<ResolveAnimationResponse> ResolveAnimationAsync(string animationId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animationId", animationId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.resolveAnimation", dict);
            return methodResult.DeserializeJson<ResolveAnimationResponse>();
        }

        /// <summary>
        /// Seek a set of animations to a particular time within each animation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SeekAnimationsAsync(string[] animations, long currentTime)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            dict.Add("currentTime", currentTime);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.seekAnimations", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets the paused state of a set of animations.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPausedAsync(string[] animations, bool paused)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            dict.Add("paused", paused);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setPaused", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets the playback rate of the document timeline.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPlaybackRateAsync(long playbackRate)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("playbackRate", playbackRate);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setPlaybackRate", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets the timing of an animation node.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTimingAsync(string animationId, long duration, long delay)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animationId", animationId);
            dict.Add("duration", duration);
            dict.Add("delay", delay);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setTiming", dict);
            return methodResult;
        }
    }
}
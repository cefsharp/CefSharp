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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Animation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables animation domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables animation domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.enable", dict);
            return methodResult;
        }

        partial void ValidateGetCurrentTime(string id);
        /// <summary>
        /// Returns the current time of the an animation.
        /// </summary>
        /// <param name = "id">Id of animation.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetCurrentTimeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetCurrentTimeResponse> GetCurrentTimeAsync(string id)
        {
            ValidateGetCurrentTime(id);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("id", id);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.getCurrentTime", dict);
            return methodResult.DeserializeJson<GetCurrentTimeResponse>();
        }

        /// <summary>
        /// Gets the playback rate of the document timeline.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetPlaybackRateResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetPlaybackRateResponse> GetPlaybackRateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.getPlaybackRate", dict);
            return methodResult.DeserializeJson<GetPlaybackRateResponse>();
        }

        partial void ValidateReleaseAnimations(string[] animations);
        /// <summary>
        /// Releases a set of animations to no longer be manipulated.
        /// </summary>
        /// <param name = "animations">List of animation ids to seek.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseAnimationsAsync(string[] animations)
        {
            ValidateReleaseAnimations(animations);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.releaseAnimations", dict);
            return methodResult;
        }

        partial void ValidateResolveAnimation(string animationId);
        /// <summary>
        /// Gets the remote object of the Animation.
        /// </summary>
        /// <param name = "animationId">Animation id.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ResolveAnimationResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ResolveAnimationResponse> ResolveAnimationAsync(string animationId)
        {
            ValidateResolveAnimation(animationId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animationId", animationId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.resolveAnimation", dict);
            return methodResult.DeserializeJson<ResolveAnimationResponse>();
        }

        partial void ValidateSeekAnimations(string[] animations, long currentTime);
        /// <summary>
        /// Seek a set of animations to a particular time within each animation.
        /// </summary>
        /// <param name = "animations">List of animation ids to seek.</param>
        /// <param name = "currentTime">Set the current time of each animation.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SeekAnimationsAsync(string[] animations, long currentTime)
        {
            ValidateSeekAnimations(animations, currentTime);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            dict.Add("currentTime", currentTime);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.seekAnimations", dict);
            return methodResult;
        }

        partial void ValidateSetPaused(string[] animations, bool paused);
        /// <summary>
        /// Sets the paused state of a set of animations.
        /// </summary>
        /// <param name = "animations">Animations to set the pause state of.</param>
        /// <param name = "paused">Paused state to set to.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPausedAsync(string[] animations, bool paused)
        {
            ValidateSetPaused(animations, paused);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animations", animations);
            dict.Add("paused", paused);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setPaused", dict);
            return methodResult;
        }

        partial void ValidateSetPlaybackRate(long playbackRate);
        /// <summary>
        /// Sets the playback rate of the document timeline.
        /// </summary>
        /// <param name = "playbackRate">Playback rate for animations on page</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPlaybackRateAsync(long playbackRate)
        {
            ValidateSetPlaybackRate(playbackRate);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("playbackRate", playbackRate);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setPlaybackRate", dict);
            return methodResult;
        }

        partial void ValidateSetTiming(string animationId, long duration, long delay);
        /// <summary>
        /// Sets the timing of an animation node.
        /// </summary>
        /// <param name = "animationId">Animation id.</param>
        /// <param name = "duration">Duration of the animation.</param>
        /// <param name = "delay">Delay of the animation.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTimingAsync(string animationId, long duration, long delay)
        {
            ValidateSetTiming(animationId, duration, delay);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("animationId", animationId);
            dict.Add("duration", duration);
            dict.Add("delay", delay);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Animation.setTiming", dict);
            return methodResult;
        }
    }
}
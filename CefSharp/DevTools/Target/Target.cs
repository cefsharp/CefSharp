// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// Supports additional targets discovery and allows to attach to them.
    /// </summary>
    public partial class Target : DevToolsDomainBase
    {
        public Target(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Activates (focuses) the target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ActivateTargetAsync(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var result = await _client.ExecuteDevToolsMethodAsync("Target.activateTarget", dict);
            return result;
        }

        /// <summary>
        /// Attaches to the target with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<AttachToTargetResponse> AttachToTargetAsync(string targetId, bool? flatten = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            if (flatten.HasValue)
            {
                dict.Add("flatten", flatten.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Target.attachToTarget", dict);
            return result.DeserializeJson<AttachToTargetResponse>();
        }

        /// <summary>
        /// Closes the target. If the target is a page that gets closed too.
        /// </summary>
        public async System.Threading.Tasks.Task<CloseTargetResponse> CloseTargetAsync(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var result = await _client.ExecuteDevToolsMethodAsync("Target.closeTarget", dict);
            return result.DeserializeJson<CloseTargetResponse>();
        }

        /// <summary>
        /// Creates a new page.
        /// </summary>
        public async System.Threading.Tasks.Task<CreateTargetResponse> CreateTargetAsync(string url, int? width = null, int? height = null, string browserContextId = null, bool? enableBeginFrameControl = null, bool? newWindow = null, bool? background = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            if (width.HasValue)
            {
                dict.Add("width", width.Value);
            }

            if (height.HasValue)
            {
                dict.Add("height", height.Value);
            }

            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            if (enableBeginFrameControl.HasValue)
            {
                dict.Add("enableBeginFrameControl", enableBeginFrameControl.Value);
            }

            if (newWindow.HasValue)
            {
                dict.Add("newWindow", newWindow.Value);
            }

            if (background.HasValue)
            {
                dict.Add("background", background.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Target.createTarget", dict);
            return result.DeserializeJson<CreateTargetResponse>();
        }

        /// <summary>
        /// Detaches session with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DetachFromTargetAsync(string sessionId = null, string targetId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(sessionId)))
            {
                dict.Add("sessionId", sessionId);
            }

            if (!(string.IsNullOrEmpty(targetId)))
            {
                dict.Add("targetId", targetId);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Target.detachFromTarget", dict);
            return result;
        }

        /// <summary>
        /// Retrieves a list of available targets.
        /// </summary>
        public async System.Threading.Tasks.Task<GetTargetsResponse> GetTargetsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Target.getTargets", dict);
            return result.DeserializeJson<GetTargetsResponse>();
        }

        /// <summary>
        /// Sends protocol message over session with given id.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SendMessageToTargetAsync(string message, string sessionId = null, string targetId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("message", message);
            if (!(string.IsNullOrEmpty(sessionId)))
            {
                dict.Add("sessionId", sessionId);
            }

            if (!(string.IsNullOrEmpty(targetId)))
            {
                dict.Add("targetId", targetId);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Target.sendMessageToTarget", dict);
            return result;
        }

        /// <summary>
        /// Controls whether to discover available targets and notify via
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDiscoverTargetsAsync(bool discover)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("discover", discover);
            var result = await _client.ExecuteDevToolsMethodAsync("Target.setDiscoverTargets", dict);
            return result;
        }
    }
}
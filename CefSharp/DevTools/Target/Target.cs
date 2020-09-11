// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    using System.Linq;

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
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ActivateTargetAsync(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.activateTarget", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.attachToTarget", dict);
            return methodResult.DeserializeJson<AttachToTargetResponse>();
        }

        /// <summary>
        /// Attaches to the browser target, only uses flat sessionId mode.
        /// </summary>
        public async System.Threading.Tasks.Task<AttachToBrowserTargetResponse> AttachToBrowserTargetAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.attachToBrowserTarget", dict);
            return methodResult.DeserializeJson<AttachToBrowserTargetResponse>();
        }

        /// <summary>
        /// Closes the target. If the target is a page that gets closed too.
        /// </summary>
        public async System.Threading.Tasks.Task<CloseTargetResponse> CloseTargetAsync(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.closeTarget", dict);
            return methodResult.DeserializeJson<CloseTargetResponse>();
        }

        /// <summary>
        /// Inject object to the target's main frame that provides a communication
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ExposeDevToolsProtocolAsync(string targetId, string bindingName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            if (!(string.IsNullOrEmpty(bindingName)))
            {
                dict.Add("bindingName", bindingName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.exposeDevToolsProtocol", dict);
            return methodResult;
        }

        /// <summary>
        /// Creates a new empty BrowserContext. Similar to an incognito profile but you can have more than
        public async System.Threading.Tasks.Task<CreateBrowserContextResponse> CreateBrowserContextAsync(bool? disposeOnDetach = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (disposeOnDetach.HasValue)
            {
                dict.Add("disposeOnDetach", disposeOnDetach.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.createBrowserContext", dict);
            return methodResult.DeserializeJson<CreateBrowserContextResponse>();
        }

        /// <summary>
        /// Returns all browser contexts created with `Target.createBrowserContext` method.
        /// </summary>
        public async System.Threading.Tasks.Task<GetBrowserContextsResponse> GetBrowserContextsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.getBrowserContexts", dict);
            return methodResult.DeserializeJson<GetBrowserContextsResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.createTarget", dict);
            return methodResult.DeserializeJson<CreateTargetResponse>();
        }

        /// <summary>
        /// Detaches session with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DetachFromTargetAsync(string sessionId = null, string targetId = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.detachFromTarget", dict);
            return methodResult;
        }

        /// <summary>
        /// Deletes a BrowserContext. All the belonging pages will be closed without calling their
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisposeBrowserContextAsync(string browserContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("browserContextId", browserContextId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.disposeBrowserContext", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns information about a target.
        /// </summary>
        public async System.Threading.Tasks.Task<GetTargetInfoResponse> GetTargetInfoAsync(string targetId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(targetId)))
            {
                dict.Add("targetId", targetId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.getTargetInfo", dict);
            return methodResult.DeserializeJson<GetTargetInfoResponse>();
        }

        /// <summary>
        /// Retrieves a list of available targets.
        /// </summary>
        public async System.Threading.Tasks.Task<GetTargetsResponse> GetTargetsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.getTargets", dict);
            return methodResult.DeserializeJson<GetTargetsResponse>();
        }

        /// <summary>
        /// Controls whether to automatically attach to new targets which are considered to be related to
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAutoAttachAsync(bool autoAttach, bool waitForDebuggerOnStart, bool? flatten = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("autoAttach", autoAttach);
            dict.Add("waitForDebuggerOnStart", waitForDebuggerOnStart);
            if (flatten.HasValue)
            {
                dict.Add("flatten", flatten.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.setAutoAttach", dict);
            return methodResult;
        }

        /// <summary>
        /// Controls whether to discover available targets and notify via
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDiscoverTargetsAsync(bool discover)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("discover", discover);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.setDiscoverTargets", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables target discovery for the specified locations, when `setDiscoverTargets` was set to
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetRemoteLocationsAsync(System.Collections.Generic.IList<CefSharp.DevTools.Target.RemoteLocation> locations)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("locations", locations.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.setRemoteLocations", dict);
            return methodResult;
        }
    }
}
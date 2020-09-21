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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Target(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateActivateTarget(string targetId);
        /// <summary>
        /// Activates (focuses) the target.
        /// </summary>
        /// <param name = "targetId">targetId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ActivateTargetAsync(string targetId)
        {
            ValidateActivateTarget(targetId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.activateTarget", dict);
            return methodResult;
        }

        partial void ValidateAttachToTarget(string targetId, bool? flatten = null);
        /// <summary>
        /// Attaches to the target with given id.
        /// </summary>
        /// <param name = "targetId">targetId</param>
        /// <param name = "flatten">Enables "flat" access to the session via specifying sessionId attribute in the commands.
        public async System.Threading.Tasks.Task<AttachToTargetResponse> AttachToTargetAsync(string targetId, bool? flatten = null)
        {
            ValidateAttachToTarget(targetId, flatten);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;AttachToBrowserTargetResponse&gt;</returns>
        public async System.Threading.Tasks.Task<AttachToBrowserTargetResponse> AttachToBrowserTargetAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.attachToBrowserTarget", dict);
            return methodResult.DeserializeJson<AttachToBrowserTargetResponse>();
        }

        partial void ValidateCloseTarget(string targetId);
        /// <summary>
        /// Closes the target. If the target is a page that gets closed too.
        /// </summary>
        /// <param name = "targetId">targetId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CloseTargetResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CloseTargetResponse> CloseTargetAsync(string targetId)
        {
            ValidateCloseTarget(targetId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.closeTarget", dict);
            return methodResult.DeserializeJson<CloseTargetResponse>();
        }

        partial void ValidateExposeDevToolsProtocol(string targetId, string bindingName = null);
        /// <summary>
        /// Inject object to the target's main frame that provides a communication
        /// channel with browser target.
        /// 
        /// Injected object will be available as `window[bindingName]`.
        /// 
        /// The object has the follwing API:
        /// - `binding.send(json)` - a method to send messages over the remote debugging protocol
        /// - `binding.onmessage = json => handleMessage(json)` - a callback that will be called for the protocol notifications and command responses.
        /// </summary>
        /// <param name = "targetId">targetId</param>
        /// <param name = "bindingName">Binding name, 'cdp' if not specified.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ExposeDevToolsProtocolAsync(string targetId, string bindingName = null)
        {
            ValidateExposeDevToolsProtocol(targetId, bindingName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("targetId", targetId);
            if (!(string.IsNullOrEmpty(bindingName)))
            {
                dict.Add("bindingName", bindingName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.exposeDevToolsProtocol", dict);
            return methodResult;
        }

        partial void ValidateCreateBrowserContext(bool? disposeOnDetach = null, string proxyServer = null, string proxyBypassList = null);
        /// <summary>
        /// Creates a new empty BrowserContext. Similar to an incognito profile but you can have more than
        /// one.
        /// </summary>
        /// <param name = "disposeOnDetach">If specified, disposes this context when debugging session disconnects.</param>
        /// <param name = "proxyServer">Proxy server, similar to the one passed to --proxy-server</param>
        /// <param name = "proxyBypassList">Proxy bypass list, similar to the one passed to --proxy-bypass-list</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CreateBrowserContextResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CreateBrowserContextResponse> CreateBrowserContextAsync(bool? disposeOnDetach = null, string proxyServer = null, string proxyBypassList = null)
        {
            ValidateCreateBrowserContext(disposeOnDetach, proxyServer, proxyBypassList);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (disposeOnDetach.HasValue)
            {
                dict.Add("disposeOnDetach", disposeOnDetach.Value);
            }

            if (!(string.IsNullOrEmpty(proxyServer)))
            {
                dict.Add("proxyServer", proxyServer);
            }

            if (!(string.IsNullOrEmpty(proxyBypassList)))
            {
                dict.Add("proxyBypassList", proxyBypassList);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.createBrowserContext", dict);
            return methodResult.DeserializeJson<CreateBrowserContextResponse>();
        }

        /// <summary>
        /// Returns all browser contexts created with `Target.createBrowserContext` method.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetBrowserContextsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetBrowserContextsResponse> GetBrowserContextsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.getBrowserContexts", dict);
            return methodResult.DeserializeJson<GetBrowserContextsResponse>();
        }

        partial void ValidateCreateTarget(string url, int? width = null, int? height = null, string browserContextId = null, bool? enableBeginFrameControl = null, bool? newWindow = null, bool? background = null);
        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name = "url">The initial URL the page will be navigated to.</param>
        /// <param name = "width">Frame width in DIP (headless chrome only).</param>
        /// <param name = "height">Frame height in DIP (headless chrome only).</param>
        /// <param name = "browserContextId">The browser context to create the page in.</param>
        /// <param name = "enableBeginFrameControl">Whether BeginFrames for this target will be controlled via DevTools (headless chrome only,
        public async System.Threading.Tasks.Task<CreateTargetResponse> CreateTargetAsync(string url, int? width = null, int? height = null, string browserContextId = null, bool? enableBeginFrameControl = null, bool? newWindow = null, bool? background = null)
        {
            ValidateCreateTarget(url, width, height, browserContextId, enableBeginFrameControl, newWindow, background);
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

        partial void ValidateDetachFromTarget(string sessionId = null, string targetId = null);
        /// <summary>
        /// Detaches session with given id.
        /// </summary>
        /// <param name = "sessionId">Session to detach.</param>
        /// <param name = "targetId">Deprecated.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DetachFromTargetAsync(string sessionId = null, string targetId = null)
        {
            ValidateDetachFromTarget(sessionId, targetId);
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

        partial void ValidateDisposeBrowserContext(string browserContextId);
        /// <summary>
        /// Deletes a BrowserContext. All the belonging pages will be closed without calling their
        /// beforeunload hooks.
        /// </summary>
        /// <param name = "browserContextId">browserContextId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisposeBrowserContextAsync(string browserContextId)
        {
            ValidateDisposeBrowserContext(browserContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("browserContextId", browserContextId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.disposeBrowserContext", dict);
            return methodResult;
        }

        partial void ValidateGetTargetInfo(string targetId = null);
        /// <summary>
        /// Returns information about a target.
        /// </summary>
        /// <param name = "targetId">targetId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetTargetInfoResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetTargetInfoResponse> GetTargetInfoAsync(string targetId = null)
        {
            ValidateGetTargetInfo(targetId);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;GetTargetsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetTargetsResponse> GetTargetsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.getTargets", dict);
            return methodResult.DeserializeJson<GetTargetsResponse>();
        }

        partial void ValidateSetAutoAttach(bool autoAttach, bool waitForDebuggerOnStart, bool? flatten = null);
        /// <summary>
        /// Controls whether to automatically attach to new targets which are considered to be related to
        /// this one. When turned on, attaches to all existing related targets as well. When turned off,
        /// automatically detaches from all currently attached targets.
        /// </summary>
        /// <param name = "autoAttach">Whether to auto-attach to related targets.</param>
        /// <param name = "waitForDebuggerOnStart">Whether to pause new targets when attaching to them. Use `Runtime.runIfWaitingForDebugger`
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAutoAttachAsync(bool autoAttach, bool waitForDebuggerOnStart, bool? flatten = null)
        {
            ValidateSetAutoAttach(autoAttach, waitForDebuggerOnStart, flatten);
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

        partial void ValidateSetDiscoverTargets(bool discover);
        /// <summary>
        /// Controls whether to discover available targets and notify via
        /// `targetCreated/targetInfoChanged/targetDestroyed` events.
        /// </summary>
        /// <param name = "discover">Whether to discover available targets.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDiscoverTargetsAsync(bool discover)
        {
            ValidateSetDiscoverTargets(discover);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("discover", discover);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.setDiscoverTargets", dict);
            return methodResult;
        }

        partial void ValidateSetRemoteLocations(System.Collections.Generic.IList<CefSharp.DevTools.Target.RemoteLocation> locations);
        /// <summary>
        /// Enables target discovery for the specified locations, when `setDiscoverTargets` was set to
        /// `true`.
        /// </summary>
        /// <param name = "locations">List of remote locations.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetRemoteLocationsAsync(System.Collections.Generic.IList<CefSharp.DevTools.Target.RemoteLocation> locations)
        {
            ValidateSetRemoteLocations(locations);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("locations", locations.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Target.setRemoteLocations", dict);
            return methodResult;
        }
    }
}
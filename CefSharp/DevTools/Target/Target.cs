// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// Supports additional targets discovery and allows to attach to them.
    /// </summary>
    public partial class Target
    {
        public Target(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Activates (focuses) the target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ActivateTarget(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.ActivateTarget", dict);
            return result;
        }

        /// <summary>
        /// Attaches to the target with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AttachToTarget(string targetId, bool flatten)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, {"flatten", flatten}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.AttachToTarget", dict);
            return result;
        }

        /// <summary>
        /// Attaches to the browser target, only uses flat sessionId mode.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AttachToBrowserTarget()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Target.AttachToBrowserTarget", dict);
            return result;
        }

        /// <summary>
        /// Closes the target. If the target is a page that gets closed too.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CloseTarget(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.CloseTarget", dict);
            return result;
        }

        /// <summary>
        /// Inject object to the target's main frame that provides a communication
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ExposeDevToolsProtocol(string targetId, string bindingName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, {"bindingName", bindingName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.ExposeDevToolsProtocol", dict);
            return result;
        }

        /// <summary>
        /// Creates a new empty BrowserContext. Similar to an incognito profile but you can have more than
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CreateBrowserContext(bool disposeOnDetach, string proxyServer, string proxyBypassList)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"disposeOnDetach", disposeOnDetach}, {"proxyServer", proxyServer}, {"proxyBypassList", proxyBypassList}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.CreateBrowserContext", dict);
            return result;
        }

        /// <summary>
        /// Returns all browser contexts created with `Target.createBrowserContext` method.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetBrowserContexts()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Target.GetBrowserContexts", dict);
            return result;
        }

        /// <summary>
        /// Creates a new page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CreateTarget(string url, int width, int height, string browserContextId, bool enableBeginFrameControl, bool newWindow, bool background)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"url", url}, {"width", width}, {"height", height}, {"browserContextId", browserContextId}, {"enableBeginFrameControl", enableBeginFrameControl}, {"newWindow", newWindow}, {"background", background}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.CreateTarget", dict);
            return result;
        }

        /// <summary>
        /// Detaches session with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DetachFromTarget(string sessionId, string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"sessionId", sessionId}, {"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.DetachFromTarget", dict);
            return result;
        }

        /// <summary>
        /// Deletes a BrowserContext. All the belonging pages will be closed without calling their
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisposeBrowserContext(string browserContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"browserContextId", browserContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.DisposeBrowserContext", dict);
            return result;
        }

        /// <summary>
        /// Returns information about a target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetTargetInfo(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.GetTargetInfo", dict);
            return result;
        }

        /// <summary>
        /// Retrieves a list of available targets.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetTargets()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Target.GetTargets", dict);
            return result;
        }

        /// <summary>
        /// Sends protocol message over session with given id.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SendMessageToTarget(string message, string sessionId, string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"message", message}, {"sessionId", sessionId}, {"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.SendMessageToTarget", dict);
            return result;
        }

        /// <summary>
        /// Controls whether to automatically attach to new targets which are considered to be related to
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAutoAttach(bool autoAttach, bool waitForDebuggerOnStart, bool flatten)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"autoAttach", autoAttach}, {"waitForDebuggerOnStart", waitForDebuggerOnStart}, {"flatten", flatten}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.SetAutoAttach", dict);
            return result;
        }

        /// <summary>
        /// Controls whether to discover available targets and notify via
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDiscoverTargets(bool discover)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"discover", discover}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.SetDiscoverTargets", dict);
            return result;
        }

        /// <summary>
        /// Enables target discovery for the specified locations, when `setDiscoverTargets` was set to
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetRemoteLocations(System.Collections.Generic.IList<RemoteLocation> locations)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"locations", locations}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Target.SetRemoteLocations", dict);
            return result;
        }
    }
}
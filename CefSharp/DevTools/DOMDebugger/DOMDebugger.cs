// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    using System.Linq;

    /// <summary>
    /// DOM debugging allows setting breakpoints on particular DOM operations and events. JavaScript
    public partial class DOMDebugger : DevToolsDomainBase
    {
        public DOMDebugger(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Returns event listeners of the given object.
        /// </summary>
        public async System.Threading.Tasks.Task<GetEventListenersResponse> GetEventListenersAsync(string objectId, int? depth = null, bool? pierce = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            if (depth.HasValue)
            {
                dict.Add("depth", depth.Value);
            }

            if (pierce.HasValue)
            {
                dict.Add("pierce", pierce.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.getEventListeners", dict);
            return result.DeserializeJson<GetEventListenersResponse>();
        }

        /// <summary>
        /// Removes DOM breakpoint that was set using `setDOMBreakpoint`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveDOMBreakpointAsync(int nodeId, DOMBreakpointType type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeDOMBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Removes breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeEventListenerBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Removes breakpoint from XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveXHRBreakpointAsync(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeXHRBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on particular operation with DOM.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDOMBreakpointAsync(int nodeId, DOMBreakpointType type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setDOMBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setEventListenerBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetXHRBreakpointAsync(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setXHRBreakpoint", dict);
            return result;
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    using System.Linq;

    /// <summary>
    /// DOM debugging allows setting breakpoints on particular DOM operations and events. JavaScript
    /// execution will stop on these operations as if there was a regular breakpoint set.
    /// </summary>
    public partial class DOMDebugger : DevToolsDomainBase
    {
        public DOMDebugger(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.getEventListeners", dict);
            return methodResult.DeserializeJson<GetEventListenersResponse>();
        }

        /// <summary>
        /// Removes DOM breakpoint that was set using `setDOMBreakpoint`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveDOMBreakpointAsync(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeDOMBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeEventListenerBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes breakpoint on particular native event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveInstrumentationBreakpointAsync(string eventName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeInstrumentationBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes breakpoint from XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveXHRBreakpointAsync(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeXHRBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets breakpoint on particular operation with DOM.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDOMBreakpointAsync(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setDOMBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setEventListenerBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets breakpoint on particular native event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInstrumentationBreakpointAsync(string eventName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setInstrumentationBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets breakpoint on XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetXHRBreakpointAsync(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setXHRBreakpoint", dict);
            return methodResult;
        }
    }
}
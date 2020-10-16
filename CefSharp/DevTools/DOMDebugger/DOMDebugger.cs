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
        private CefSharp.DevTools.IDevToolsClient _client;
        public DOMDebugger(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateGetEventListeners(string objectId, int? depth = null, bool? pierce = null);
        /// <summary>
        /// Returns event listeners of the given object.
        /// </summary>
        /// <param name = "objectId">Identifier of the object to return listeners for.</param>
        /// <param name = "depth">The maximum depth at which Node children should be retrieved, defaults to 1. Use -1 for theentire subtree or provide an integer larger than 0.</param>
        /// <param name = "pierce">Whether or not iframes and shadow roots should be traversed when returning the subtree(default is false). Reports listeners for all contexts if pierce is enabled.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetEventListenersResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetEventListenersResponse> GetEventListenersAsync(string objectId, int? depth = null, bool? pierce = null)
        {
            ValidateGetEventListeners(objectId, depth, pierce);
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

        partial void ValidateRemoveDOMBreakpoint(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type);
        /// <summary>
        /// Removes DOM breakpoint that was set using `setDOMBreakpoint`.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node to remove breakpoint from.</param>
        /// <param name = "type">Type of the breakpoint to remove.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveDOMBreakpointAsync(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type)
        {
            ValidateRemoveDOMBreakpoint(nodeId, type);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeDOMBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateRemoveEventListenerBreakpoint(string eventName, string targetName = null);
        /// <summary>
        /// Removes breakpoint on particular DOM event.
        /// </summary>
        /// <param name = "eventName">Event name.</param>
        /// <param name = "targetName">EventTarget interface name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            ValidateRemoveEventListenerBreakpoint(eventName, targetName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeEventListenerBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateRemoveInstrumentationBreakpoint(string eventName);
        /// <summary>
        /// Removes breakpoint on particular native event.
        /// </summary>
        /// <param name = "eventName">Instrumentation name to stop on.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveInstrumentationBreakpointAsync(string eventName)
        {
            ValidateRemoveInstrumentationBreakpoint(eventName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeInstrumentationBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateRemoveXHRBreakpoint(string url);
        /// <summary>
        /// Removes breakpoint from XMLHttpRequest.
        /// </summary>
        /// <param name = "url">Resource URL substring.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveXHRBreakpointAsync(string url)
        {
            ValidateRemoveXHRBreakpoint(url);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.removeXHRBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateSetDOMBreakpoint(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type);
        /// <summary>
        /// Sets breakpoint on particular operation with DOM.
        /// </summary>
        /// <param name = "nodeId">Identifier of the node to set breakpoint on.</param>
        /// <param name = "type">Type of the operation to stop upon.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDOMBreakpointAsync(int nodeId, CefSharp.DevTools.DOMDebugger.DOMBreakpointType type)
        {
            ValidateSetDOMBreakpoint(nodeId, type);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("type", this.EnumToString(type));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setDOMBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateSetEventListenerBreakpoint(string eventName, string targetName = null);
        /// <summary>
        /// Sets breakpoint on particular DOM event.
        /// </summary>
        /// <param name = "eventName">DOM Event name to stop on (any DOM event will do).</param>
        /// <param name = "targetName">EventTarget interface name to stop on. If equal to `"*"` or not provided, will stop on anyEventTarget.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEventListenerBreakpointAsync(string eventName, string targetName = null)
        {
            ValidateSetEventListenerBreakpoint(eventName, targetName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            if (!(string.IsNullOrEmpty(targetName)))
            {
                dict.Add("targetName", targetName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setEventListenerBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateSetInstrumentationBreakpoint(string eventName);
        /// <summary>
        /// Sets breakpoint on particular native event.
        /// </summary>
        /// <param name = "eventName">Instrumentation name to stop on.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInstrumentationBreakpointAsync(string eventName)
        {
            ValidateSetInstrumentationBreakpoint(eventName);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("eventName", eventName);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setInstrumentationBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateSetXHRBreakpoint(string url);
        /// <summary>
        /// Sets breakpoint on XMLHttpRequest.
        /// </summary>
        /// <param name = "url">Resource URL substring. All XHRs having this substring in the URL will get stopped upon.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetXHRBreakpointAsync(string url)
        {
            ValidateSetXHRBreakpoint(url);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.setXHRBreakpoint", dict);
            return methodResult;
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// DOM debugging allows setting breakpoints on particular DOM operations and events. JavaScript
    public partial class DOMDebugger
    {
        public DOMDebugger(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Returns event listeners of the given object.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetEventListeners(string objectId, int depth, bool pierce)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, {"depth", depth}, {"pierce", pierce}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.GetEventListeners", dict);
            return result;
        }

        /// <summary>
        /// Removes DOM breakpoint that was set using `setDOMBreakpoint`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveDOMBreakpoint(int nodeId, string type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"type", type}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.RemoveDOMBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Removes breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveEventListenerBreakpoint(string eventName, string targetName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"eventName", eventName}, {"targetName", targetName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.RemoveEventListenerBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Removes breakpoint on particular native event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveInstrumentationBreakpoint(string eventName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"eventName", eventName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.RemoveInstrumentationBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Removes breakpoint from XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveXHRBreakpoint(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"url", url}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.RemoveXHRBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on particular operation with DOM.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDOMBreakpoint(int nodeId, string type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"nodeId", nodeId}, {"type", type}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.SetDOMBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on particular DOM event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEventListenerBreakpoint(string eventName, string targetName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"eventName", eventName}, {"targetName", targetName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.SetEventListenerBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on particular native event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetInstrumentationBreakpoint(string eventName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"eventName", eventName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.SetInstrumentationBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets breakpoint on XMLHttpRequest.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetXHRBreakpoint(string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"url", url}, };
            var result = await _client.ExecuteDevToolsMethodAsync("DOMDebugger.SetXHRBreakpoint", dict);
            return result;
        }
    }
}
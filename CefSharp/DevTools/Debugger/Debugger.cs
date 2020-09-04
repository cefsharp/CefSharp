// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Debugger domain exposes JavaScript debugging capabilities. It allows setting and removing
    public partial class Debugger : DevToolsDomainBase
    {
        public Debugger(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Continues execution until specific location is reached.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ContinueToLocationAsync(Location location, string targetCallFrames = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("location", location);
            if (!(string.IsNullOrEmpty(targetCallFrames)))
            {
                dict.Add("targetCallFrames", targetCallFrames);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.continueToLocation", dict);
            return result;
        }

        /// <summary>
        /// Disables debugger for given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables debugger for the given page. Clients should not assume that the debugging has been
        public async System.Threading.Tasks.Task<EnableResponse> EnableAsync(long? maxScriptsCacheSize = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (maxScriptsCacheSize.HasValue)
            {
                dict.Add("maxScriptsCacheSize", maxScriptsCacheSize.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.enable", dict);
            return result.DeserializeJson<EnableResponse>();
        }

        /// <summary>
        /// Evaluates expression on a given call frame.
        /// </summary>
        public async System.Threading.Tasks.Task<EvaluateOnCallFrameResponse> EvaluateOnCallFrameAsync(string callFrameId, string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? throwOnSideEffect = null, long? timeout = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("callFrameId", callFrameId);
            dict.Add("expression", expression);
            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            if (includeCommandLineAPI.HasValue)
            {
                dict.Add("includeCommandLineAPI", includeCommandLineAPI.Value);
            }

            if (silent.HasValue)
            {
                dict.Add("silent", silent.Value);
            }

            if (returnByValue.HasValue)
            {
                dict.Add("returnByValue", returnByValue.Value);
            }

            if (generatePreview.HasValue)
            {
                dict.Add("generatePreview", generatePreview.Value);
            }

            if (throwOnSideEffect.HasValue)
            {
                dict.Add("throwOnSideEffect", throwOnSideEffect.Value);
            }

            if (timeout.HasValue)
            {
                dict.Add("timeout", timeout.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.evaluateOnCallFrame", dict);
            return result.DeserializeJson<EvaluateOnCallFrameResponse>();
        }

        /// <summary>
        /// Returns possible locations for breakpoint. scriptId in start and end range locations should be
        public async System.Threading.Tasks.Task<GetPossibleBreakpointsResponse> GetPossibleBreakpointsAsync(Location start, Location end = null, bool? restrictToFunction = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("start", start);
            if ((end) != (null))
            {
                dict.Add("end", end);
            }

            if (restrictToFunction.HasValue)
            {
                dict.Add("restrictToFunction", restrictToFunction.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.getPossibleBreakpoints", dict);
            return result.DeserializeJson<GetPossibleBreakpointsResponse>();
        }

        /// <summary>
        /// Returns source for the script with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<GetScriptSourceResponse> GetScriptSourceAsync(string scriptId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.getScriptSource", dict);
            return result.DeserializeJson<GetScriptSourceResponse>();
        }

        /// <summary>
        /// This command is deprecated. Use getScriptSource instead.
        /// </summary>
        public async System.Threading.Tasks.Task<GetWasmBytecodeResponse> GetWasmBytecodeAsync(string scriptId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.getWasmBytecode", dict);
            return result.DeserializeJson<GetWasmBytecodeResponse>();
        }

        /// <summary>
        /// Stops on the next JavaScript statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PauseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.pause", dict);
            return result;
        }

        /// <summary>
        /// Removes JavaScript breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveBreakpointAsync(string breakpointId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("breakpointId", breakpointId);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.removeBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Restarts particular call frame from the beginning.
        /// </summary>
        public async System.Threading.Tasks.Task<RestartFrameResponse> RestartFrameAsync(string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("callFrameId", callFrameId);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.restartFrame", dict);
            return result.DeserializeJson<RestartFrameResponse>();
        }

        /// <summary>
        /// Resumes JavaScript execution.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResumeAsync(bool? terminateOnResume = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (terminateOnResume.HasValue)
            {
                dict.Add("terminateOnResume", terminateOnResume.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.resume", dict);
            return result;
        }

        /// <summary>
        /// Searches for given string in script content.
        /// </summary>
        public async System.Threading.Tasks.Task<SearchInContentResponse> SearchInContentAsync(string scriptId, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            dict.Add("query", query);
            if (caseSensitive.HasValue)
            {
                dict.Add("caseSensitive", caseSensitive.Value);
            }

            if (isRegex.HasValue)
            {
                dict.Add("isRegex", isRegex.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.searchInContent", dict);
            return result.DeserializeJson<SearchInContentResponse>();
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAsyncCallStackDepthAsync(int maxDepth)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxDepth", maxDepth);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setAsyncCallStackDepth", dict);
            return result;
        }

        /// <summary>
        /// Sets JavaScript breakpoint at a given location.
        /// </summary>
        public async System.Threading.Tasks.Task<SetBreakpointResponse> SetBreakpointAsync(Location location, string condition = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("location", location);
            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpoint", dict);
            return result.DeserializeJson<SetBreakpointResponse>();
        }

        /// <summary>
        /// Sets instrumentation breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<SetInstrumentationBreakpointResponse> SetInstrumentationBreakpointAsync(string instrumentation)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("instrumentation", instrumentation);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setInstrumentationBreakpoint", dict);
            return result.DeserializeJson<SetInstrumentationBreakpointResponse>();
        }

        /// <summary>
        /// Sets JavaScript breakpoint at given location specified either by URL or URL regex. Once this
        public async System.Threading.Tasks.Task<SetBreakpointByUrlResponse> SetBreakpointByUrlAsync(int lineNumber, string url = null, string urlRegex = null, string scriptHash = null, int? columnNumber = null, string condition = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("lineNumber", lineNumber);
            if (!(string.IsNullOrEmpty(url)))
            {
                dict.Add("url", url);
            }

            if (!(string.IsNullOrEmpty(urlRegex)))
            {
                dict.Add("urlRegex", urlRegex);
            }

            if (!(string.IsNullOrEmpty(scriptHash)))
            {
                dict.Add("scriptHash", scriptHash);
            }

            if (columnNumber.HasValue)
            {
                dict.Add("columnNumber", columnNumber.Value);
            }

            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointByUrl", dict);
            return result.DeserializeJson<SetBreakpointByUrlResponse>();
        }

        /// <summary>
        /// Activates / deactivates all breakpoints on the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBreakpointsActiveAsync(bool active)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("active", active);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointsActive", dict);
            return result;
        }

        /// <summary>
        /// Defines pause on exceptions state. Can be set to stop on all exceptions, uncaught exceptions or
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetPauseOnExceptionsAsync(string state)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("state", state);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setPauseOnExceptions", dict);
            return result;
        }

        /// <summary>
        /// Edits JavaScript source live.
        /// </summary>
        public async System.Threading.Tasks.Task<SetScriptSourceResponse> SetScriptSourceAsync(string scriptId, string scriptSource, bool? dryRun = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            dict.Add("scriptSource", scriptSource);
            if (dryRun.HasValue)
            {
                dict.Add("dryRun", dryRun.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setScriptSource", dict);
            return result.DeserializeJson<SetScriptSourceResponse>();
        }

        /// <summary>
        /// Makes page not interrupt on any pauses (breakpoint, exception, dom exception etc).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetSkipAllPausesAsync(bool skip)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("skip", skip);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setSkipAllPauses", dict);
            return result;
        }

        /// <summary>
        /// Changes value of variable in a callframe. Object-based scopes are not supported and must be
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetVariableValueAsync(int scopeNumber, string variableName, Runtime.CallArgument newValue, string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeNumber", scopeNumber);
            dict.Add("variableName", variableName);
            dict.Add("newValue", newValue);
            dict.Add("callFrameId", callFrameId);
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.setVariableValue", dict);
            return result;
        }

        /// <summary>
        /// Steps into the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepIntoAsync(bool? breakOnAsyncCall = null, System.Collections.Generic.IList<LocationRange> skipList = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (breakOnAsyncCall.HasValue)
            {
                dict.Add("breakOnAsyncCall", breakOnAsyncCall.Value);
            }

            if ((skipList) != (null))
            {
                dict.Add("skipList", skipList);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.stepInto", dict);
            return result;
        }

        /// <summary>
        /// Steps out of the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepOutAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOut", dict);
            return result;
        }

        /// <summary>
        /// Steps over the statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepOverAsync(System.Collections.Generic.IList<LocationRange> skipList = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((skipList) != (null))
            {
                dict.Add("skipList", skipList);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOver", dict);
            return result;
        }
    }
}
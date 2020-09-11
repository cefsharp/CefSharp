// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    using System.Linq;

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
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueToLocationAsync(CefSharp.DevTools.Debugger.Location location, string targetCallFrames = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("location", location.ToDictionary());
            if (!(string.IsNullOrEmpty(targetCallFrames)))
            {
                dict.Add("targetCallFrames", targetCallFrames);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.continueToLocation", dict);
            return methodResult;
        }

        /// <summary>
        /// Disables debugger for given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.disable", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.enable", dict);
            return methodResult.DeserializeJson<EnableResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.evaluateOnCallFrame", dict);
            return methodResult.DeserializeJson<EvaluateOnCallFrameResponse>();
        }

        /// <summary>
        /// Execute a Wasm Evaluator module on a given call frame.
        /// </summary>
        public async System.Threading.Tasks.Task<ExecuteWasmEvaluatorResponse> ExecuteWasmEvaluatorAsync(string callFrameId, byte[] evaluator, long? timeout = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("callFrameId", callFrameId);
            dict.Add("evaluator", ToBase64String(evaluator));
            if (timeout.HasValue)
            {
                dict.Add("timeout", timeout.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.executeWasmEvaluator", dict);
            return methodResult.DeserializeJson<ExecuteWasmEvaluatorResponse>();
        }

        /// <summary>
        /// Returns possible locations for breakpoint. scriptId in start and end range locations should be
        public async System.Threading.Tasks.Task<GetPossibleBreakpointsResponse> GetPossibleBreakpointsAsync(CefSharp.DevTools.Debugger.Location start, CefSharp.DevTools.Debugger.Location end = null, bool? restrictToFunction = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("start", start.ToDictionary());
            if ((end) != (null))
            {
                dict.Add("end", end.ToDictionary());
            }

            if (restrictToFunction.HasValue)
            {
                dict.Add("restrictToFunction", restrictToFunction.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.getPossibleBreakpoints", dict);
            return methodResult.DeserializeJson<GetPossibleBreakpointsResponse>();
        }

        /// <summary>
        /// Returns source for the script with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<GetScriptSourceResponse> GetScriptSourceAsync(string scriptId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.getScriptSource", dict);
            return methodResult.DeserializeJson<GetScriptSourceResponse>();
        }

        /// <summary>
        /// Returns stack trace with given `stackTraceId`.
        /// </summary>
        public async System.Threading.Tasks.Task<GetStackTraceResponse> GetStackTraceAsync(CefSharp.DevTools.Runtime.StackTraceId stackTraceId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("stackTraceId", stackTraceId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.getStackTrace", dict);
            return methodResult.DeserializeJson<GetStackTraceResponse>();
        }

        /// <summary>
        /// Stops on the next JavaScript statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> PauseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.pause", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes JavaScript breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveBreakpointAsync(string breakpointId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("breakpointId", breakpointId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.removeBreakpoint", dict);
            return methodResult;
        }

        /// <summary>
        /// Restarts particular call frame from the beginning.
        /// </summary>
        public async System.Threading.Tasks.Task<RestartFrameResponse> RestartFrameAsync(string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("callFrameId", callFrameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.restartFrame", dict);
            return methodResult.DeserializeJson<RestartFrameResponse>();
        }

        /// <summary>
        /// Resumes JavaScript execution.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResumeAsync(bool? terminateOnResume = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (terminateOnResume.HasValue)
            {
                dict.Add("terminateOnResume", terminateOnResume.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.resume", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.searchInContent", dict);
            return methodResult.DeserializeJson<SearchInContentResponse>();
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAsyncCallStackDepthAsync(int maxDepth)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxDepth", maxDepth);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setAsyncCallStackDepth", dict);
            return methodResult;
        }

        /// <summary>
        /// Replace previous blackbox patterns with passed ones. Forces backend to skip stepping/pausing in
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlackboxPatternsAsync(string[] patterns)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("patterns", patterns);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBlackboxPatterns", dict);
            return methodResult;
        }

        /// <summary>
        /// Makes backend skip steps in the script in blackboxed ranges. VM will try leave blacklisted
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlackboxedRangesAsync(string scriptId, System.Collections.Generic.IList<CefSharp.DevTools.Debugger.ScriptPosition> positions)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            dict.Add("positions", positions.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBlackboxedRanges", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets JavaScript breakpoint at a given location.
        /// </summary>
        public async System.Threading.Tasks.Task<SetBreakpointResponse> SetBreakpointAsync(CefSharp.DevTools.Debugger.Location location, string condition = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("location", location.ToDictionary());
            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpoint", dict);
            return methodResult.DeserializeJson<SetBreakpointResponse>();
        }

        /// <summary>
        /// Sets instrumentation breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<SetInstrumentationBreakpointResponse> SetInstrumentationBreakpointAsync(string instrumentation)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("instrumentation", instrumentation);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setInstrumentationBreakpoint", dict);
            return methodResult.DeserializeJson<SetInstrumentationBreakpointResponse>();
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointByUrl", dict);
            return methodResult.DeserializeJson<SetBreakpointByUrlResponse>();
        }

        /// <summary>
        /// Sets JavaScript breakpoint before each call to the given function.
        public async System.Threading.Tasks.Task<SetBreakpointOnFunctionCallResponse> SetBreakpointOnFunctionCallAsync(string objectId, string condition = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointOnFunctionCall", dict);
            return methodResult.DeserializeJson<SetBreakpointOnFunctionCallResponse>();
        }

        /// <summary>
        /// Activates / deactivates all breakpoints on the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBreakpointsActiveAsync(bool active)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("active", active);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointsActive", dict);
            return methodResult;
        }

        /// <summary>
        /// Defines pause on exceptions state. Can be set to stop on all exceptions, uncaught exceptions or
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPauseOnExceptionsAsync(string state)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("state", state);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setPauseOnExceptions", dict);
            return methodResult;
        }

        /// <summary>
        /// Changes return value in top frame. Available only at return break position.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetReturnValueAsync(CefSharp.DevTools.Runtime.CallArgument newValue)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("newValue", newValue.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setReturnValue", dict);
            return methodResult;
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setScriptSource", dict);
            return methodResult.DeserializeJson<SetScriptSourceResponse>();
        }

        /// <summary>
        /// Makes page not interrupt on any pauses (breakpoint, exception, dom exception etc).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetSkipAllPausesAsync(bool skip)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("skip", skip);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setSkipAllPauses", dict);
            return methodResult;
        }

        /// <summary>
        /// Changes value of variable in a callframe. Object-based scopes are not supported and must be
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetVariableValueAsync(int scopeNumber, string variableName, CefSharp.DevTools.Runtime.CallArgument newValue, string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeNumber", scopeNumber);
            dict.Add("variableName", variableName);
            dict.Add("newValue", newValue.ToDictionary());
            dict.Add("callFrameId", callFrameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setVariableValue", dict);
            return methodResult;
        }

        /// <summary>
        /// Steps into the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepIntoAsync(bool? breakOnAsyncCall = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (breakOnAsyncCall.HasValue)
            {
                dict.Add("breakOnAsyncCall", breakOnAsyncCall.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.stepInto", dict);
            return methodResult;
        }

        /// <summary>
        /// Steps out of the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepOutAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOut", dict);
            return methodResult;
        }

        /// <summary>
        /// Steps over the statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepOverAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOver", dict);
            return methodResult;
        }
    }
}
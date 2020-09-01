// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Debugger domain exposes JavaScript debugging capabilities. It allows setting and removing
    public partial class Debugger
    {
        public Debugger(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Continues execution until specific location is reached.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ContinueToLocation(Location location, string targetCallFrames)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"location", location}, {"targetCallFrames", targetCallFrames}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.ContinueToLocation", dict);
            return result;
        }

        /// <summary>
        /// Disables debugger for given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enables debugger for the given page. Clients should not assume that the debugging has been
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable(long maxScriptsCacheSize)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"maxScriptsCacheSize", maxScriptsCacheSize}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.Enable", dict);
            return result;
        }

        /// <summary>
        /// Evaluates expression on a given call frame.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EvaluateOnCallFrame(string callFrameId, string expression, string objectGroup, bool includeCommandLineAPI, bool silent, bool returnByValue, bool generatePreview, bool throwOnSideEffect, long timeout)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"callFrameId", callFrameId}, {"expression", expression}, {"objectGroup", objectGroup}, {"includeCommandLineAPI", includeCommandLineAPI}, {"silent", silent}, {"returnByValue", returnByValue}, {"generatePreview", generatePreview}, {"throwOnSideEffect", throwOnSideEffect}, {"timeout", timeout}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.EvaluateOnCallFrame", dict);
            return result;
        }

        /// <summary>
        /// Execute a Wasm Evaluator module on a given call frame.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ExecuteWasmEvaluator(string callFrameId, string evaluator, long timeout)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"callFrameId", callFrameId}, {"evaluator", evaluator}, {"timeout", timeout}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.ExecuteWasmEvaluator", dict);
            return result;
        }

        /// <summary>
        /// Returns possible locations for breakpoint. scriptId in start and end range locations should be
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetPossibleBreakpoints(Location start, Location end, bool restrictToFunction)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"start", start}, {"end", end}, {"restrictToFunction", restrictToFunction}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.GetPossibleBreakpoints", dict);
            return result;
        }

        /// <summary>
        /// Returns source for the script with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetScriptSource(string scriptId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.GetScriptSource", dict);
            return result;
        }

        /// <summary>
        /// This command is deprecated. Use getScriptSource instead.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetWasmBytecode(string scriptId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.GetWasmBytecode", dict);
            return result;
        }

        /// <summary>
        /// Returns stack trace with given `stackTraceId`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetStackTrace(Runtime.StackTraceId stackTraceId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"stackTraceId", stackTraceId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.GetStackTrace", dict);
            return result;
        }

        /// <summary>
        /// Stops on the next JavaScript statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Pause()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.Pause", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PauseOnAsyncCall(Runtime.StackTraceId parentStackTraceId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"parentStackTraceId", parentStackTraceId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.PauseOnAsyncCall", dict);
            return result;
        }

        /// <summary>
        /// Removes JavaScript breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveBreakpoint(string breakpointId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"breakpointId", breakpointId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.RemoveBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Restarts particular call frame from the beginning.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RestartFrame(string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"callFrameId", callFrameId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.RestartFrame", dict);
            return result;
        }

        /// <summary>
        /// Resumes JavaScript execution.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Resume(bool terminateOnResume)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"terminateOnResume", terminateOnResume}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.Resume", dict);
            return result;
        }

        /// <summary>
        /// Searches for given string in script content.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SearchInContent(string scriptId, string query, bool caseSensitive, bool isRegex)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, {"query", query}, {"caseSensitive", caseSensitive}, {"isRegex", isRegex}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SearchInContent", dict);
            return result;
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAsyncCallStackDepth(int maxDepth)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"maxDepth", maxDepth}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetAsyncCallStackDepth", dict);
            return result;
        }

        /// <summary>
        /// Replace previous blackbox patterns with passed ones. Forces backend to skip stepping/pausing in
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBlackboxPatterns(string patterns)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"patterns", patterns}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBlackboxPatterns", dict);
            return result;
        }

        /// <summary>
        /// Makes backend skip steps in the script in blackboxed ranges. VM will try leave blacklisted
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBlackboxedRanges(string scriptId, System.Collections.Generic.IList<ScriptPosition> positions)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, {"positions", positions}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBlackboxedRanges", dict);
            return result;
        }

        /// <summary>
        /// Sets JavaScript breakpoint at a given location.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBreakpoint(Location location, string condition)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"location", location}, {"condition", condition}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets instrumentation breakpoint.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetInstrumentationBreakpoint(string instrumentation)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"instrumentation", instrumentation}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetInstrumentationBreakpoint", dict);
            return result;
        }

        /// <summary>
        /// Sets JavaScript breakpoint at given location specified either by URL or URL regex. Once this
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBreakpointByUrl(int lineNumber, string url, string urlRegex, string scriptHash, int columnNumber, string condition)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"lineNumber", lineNumber}, {"url", url}, {"urlRegex", urlRegex}, {"scriptHash", scriptHash}, {"columnNumber", columnNumber}, {"condition", condition}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBreakpointByUrl", dict);
            return result;
        }

        /// <summary>
        /// Sets JavaScript breakpoint before each call to the given function.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBreakpointOnFunctionCall(string objectId, string condition)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, {"condition", condition}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBreakpointOnFunctionCall", dict);
            return result;
        }

        /// <summary>
        /// Activates / deactivates all breakpoints on the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBreakpointsActive(bool active)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"active", active}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetBreakpointsActive", dict);
            return result;
        }

        /// <summary>
        /// Defines pause on exceptions state. Can be set to stop on all exceptions, uncaught exceptions or
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetPauseOnExceptions(string state)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"state", state}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetPauseOnExceptions", dict);
            return result;
        }

        /// <summary>
        /// Changes return value in top frame. Available only at return break position.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetReturnValue(Runtime.CallArgument newValue)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"newValue", newValue}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetReturnValue", dict);
            return result;
        }

        /// <summary>
        /// Edits JavaScript source live.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetScriptSource(string scriptId, string scriptSource, bool dryRun)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, {"scriptSource", scriptSource}, {"dryRun", dryRun}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetScriptSource", dict);
            return result;
        }

        /// <summary>
        /// Makes page not interrupt on any pauses (breakpoint, exception, dom exception etc).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetSkipAllPauses(bool skip)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"skip", skip}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetSkipAllPauses", dict);
            return result;
        }

        /// <summary>
        /// Changes value of variable in a callframe. Object-based scopes are not supported and must be
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetVariableValue(int scopeNumber, string variableName, Runtime.CallArgument newValue, string callFrameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scopeNumber", scopeNumber}, {"variableName", variableName}, {"newValue", newValue}, {"callFrameId", callFrameId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.SetVariableValue", dict);
            return result;
        }

        /// <summary>
        /// Steps into the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepInto(bool breakOnAsyncCall, System.Collections.Generic.IList<LocationRange> skipList)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"breakOnAsyncCall", breakOnAsyncCall}, {"skipList", skipList}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.StepInto", dict);
            return result;
        }

        /// <summary>
        /// Steps out of the function call.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepOut()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.StepOut", dict);
            return result;
        }

        /// <summary>
        /// Steps over the statement.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StepOver(System.Collections.Generic.IList<LocationRange> skipList)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"skipList", skipList}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Debugger.StepOver", dict);
            return result;
        }
    }
}
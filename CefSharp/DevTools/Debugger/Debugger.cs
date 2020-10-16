// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    using System.Linq;

    /// <summary>
    /// Debugger domain exposes JavaScript debugging capabilities. It allows setting and removing
    /// breakpoints, stepping through execution, exploring stack traces, etc.
    /// </summary>
    public partial class Debugger : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Debugger(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateContinueToLocation(CefSharp.DevTools.Debugger.Location location, string targetCallFrames = null);
        /// <summary>
        /// Continues execution until specific location is reached.
        /// </summary>
        /// <param name = "location">Location to continue to.</param>
        /// <param name = "targetCallFrames">targetCallFrames</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ContinueToLocationAsync(CefSharp.DevTools.Debugger.Location location, string targetCallFrames = null)
        {
            ValidateContinueToLocation(location, targetCallFrames);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.disable", dict);
            return methodResult;
        }

        partial void ValidateEnable(long? maxScriptsCacheSize = null);
        /// <summary>
        /// Enables debugger for the given page. Clients should not assume that the debugging has been
        /// enabled until the result for this command is received.
        /// </summary>
        /// <param name = "maxScriptsCacheSize">The maximum size in bytes of collected scripts (not referenced by other heap objects)the debugger can hold. Puts no limit if paramter is omitted.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;EnableResponse&gt;</returns>
        public async System.Threading.Tasks.Task<EnableResponse> EnableAsync(long? maxScriptsCacheSize = null)
        {
            ValidateEnable(maxScriptsCacheSize);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (maxScriptsCacheSize.HasValue)
            {
                dict.Add("maxScriptsCacheSize", maxScriptsCacheSize.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.enable", dict);
            return methodResult.DeserializeJson<EnableResponse>();
        }

        partial void ValidateEvaluateOnCallFrame(string callFrameId, string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? throwOnSideEffect = null, long? timeout = null);
        /// <summary>
        /// Evaluates expression on a given call frame.
        /// </summary>
        /// <param name = "callFrameId">Call frame identifier to evaluate on.</param>
        /// <param name = "expression">Expression to evaluate.</param>
        /// <param name = "objectGroup">String object group name to put result into (allows rapid releasing resulting object handlesusing `releaseObjectGroup`).</param>
        /// <param name = "includeCommandLineAPI">Specifies whether command line API should be available to the evaluated expression, defaultsto false.</param>
        /// <param name = "silent">In silent mode exceptions thrown during evaluation are not reported and do not pauseexecution. Overrides `setPauseOnException` state.</param>
        /// <param name = "returnByValue">Whether the result is expected to be a JSON object that should be sent by value.</param>
        /// <param name = "generatePreview">Whether preview should be generated for the result.</param>
        /// <param name = "throwOnSideEffect">Whether to throw an exception if side effect cannot be ruled out during evaluation.</param>
        /// <param name = "timeout">Terminate execution after timing out (number of milliseconds).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;EvaluateOnCallFrameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<EvaluateOnCallFrameResponse> EvaluateOnCallFrameAsync(string callFrameId, string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? throwOnSideEffect = null, long? timeout = null)
        {
            ValidateEvaluateOnCallFrame(callFrameId, expression, objectGroup, includeCommandLineAPI, silent, returnByValue, generatePreview, throwOnSideEffect, timeout);
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

        partial void ValidateExecuteWasmEvaluator(string callFrameId, byte[] evaluator, long? timeout = null);
        /// <summary>
        /// Execute a Wasm Evaluator module on a given call frame.
        /// </summary>
        /// <param name = "callFrameId">WebAssembly call frame identifier to evaluate on.</param>
        /// <param name = "evaluator">Code of the evaluator module.</param>
        /// <param name = "timeout">Terminate execution after timing out (number of milliseconds).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;ExecuteWasmEvaluatorResponse&gt;</returns>
        public async System.Threading.Tasks.Task<ExecuteWasmEvaluatorResponse> ExecuteWasmEvaluatorAsync(string callFrameId, byte[] evaluator, long? timeout = null)
        {
            ValidateExecuteWasmEvaluator(callFrameId, evaluator, timeout);
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

        partial void ValidateGetPossibleBreakpoints(CefSharp.DevTools.Debugger.Location start, CefSharp.DevTools.Debugger.Location end = null, bool? restrictToFunction = null);
        /// <summary>
        /// Returns possible locations for breakpoint. scriptId in start and end range locations should be
        /// the same.
        /// </summary>
        /// <param name = "start">Start of range to search possible breakpoint locations in.</param>
        /// <param name = "end">End of range to search possible breakpoint locations in (excluding). When not specified, endof scripts is used as end of range.</param>
        /// <param name = "restrictToFunction">Only consider locations which are in the same (non-nested) function as start.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetPossibleBreakpointsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetPossibleBreakpointsResponse> GetPossibleBreakpointsAsync(CefSharp.DevTools.Debugger.Location start, CefSharp.DevTools.Debugger.Location end = null, bool? restrictToFunction = null)
        {
            ValidateGetPossibleBreakpoints(start, end, restrictToFunction);
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

        partial void ValidateGetScriptSource(string scriptId);
        /// <summary>
        /// Returns source for the script with given id.
        /// </summary>
        /// <param name = "scriptId">Id of the script to get source for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetScriptSourceResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetScriptSourceResponse> GetScriptSourceAsync(string scriptId)
        {
            ValidateGetScriptSource(scriptId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.getScriptSource", dict);
            return methodResult.DeserializeJson<GetScriptSourceResponse>();
        }

        partial void ValidateGetStackTrace(CefSharp.DevTools.Runtime.StackTraceId stackTraceId);
        /// <summary>
        /// Returns stack trace with given `stackTraceId`.
        /// </summary>
        /// <param name = "stackTraceId">stackTraceId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetStackTraceResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetStackTraceResponse> GetStackTraceAsync(CefSharp.DevTools.Runtime.StackTraceId stackTraceId)
        {
            ValidateGetStackTrace(stackTraceId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("stackTraceId", stackTraceId.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.getStackTrace", dict);
            return methodResult.DeserializeJson<GetStackTraceResponse>();
        }

        /// <summary>
        /// Stops on the next JavaScript statement.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> PauseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.pause", dict);
            return methodResult;
        }

        partial void ValidateRemoveBreakpoint(string breakpointId);
        /// <summary>
        /// Removes JavaScript breakpoint.
        /// </summary>
        /// <param name = "breakpointId">breakpointId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveBreakpointAsync(string breakpointId)
        {
            ValidateRemoveBreakpoint(breakpointId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("breakpointId", breakpointId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.removeBreakpoint", dict);
            return methodResult;
        }

        partial void ValidateRestartFrame(string callFrameId);
        /// <summary>
        /// Restarts particular call frame from the beginning.
        /// </summary>
        /// <param name = "callFrameId">Call frame identifier to evaluate on.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;RestartFrameResponse&gt;</returns>
        public async System.Threading.Tasks.Task<RestartFrameResponse> RestartFrameAsync(string callFrameId)
        {
            ValidateRestartFrame(callFrameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("callFrameId", callFrameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.restartFrame", dict);
            return methodResult.DeserializeJson<RestartFrameResponse>();
        }

        partial void ValidateResume(bool? terminateOnResume = null);
        /// <summary>
        /// Resumes JavaScript execution.
        /// </summary>
        /// <param name = "terminateOnResume">Set to true to terminate execution upon resuming execution. In contrastto Runtime.terminateExecution, this will allows to execute furtherJavaScript (i.e. via evaluation) until execution of the paused codeis actually resumed, at which point termination is triggered.If execution is currently not paused, this parameter has no effect.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResumeAsync(bool? terminateOnResume = null)
        {
            ValidateResume(terminateOnResume);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (terminateOnResume.HasValue)
            {
                dict.Add("terminateOnResume", terminateOnResume.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.resume", dict);
            return methodResult;
        }

        partial void ValidateSearchInContent(string scriptId, string query, bool? caseSensitive = null, bool? isRegex = null);
        /// <summary>
        /// Searches for given string in script content.
        /// </summary>
        /// <param name = "scriptId">Id of the script to search in.</param>
        /// <param name = "query">String to search for.</param>
        /// <param name = "caseSensitive">If true, search is case sensitive.</param>
        /// <param name = "isRegex">If true, treats string parameter as regex.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SearchInContentResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SearchInContentResponse> SearchInContentAsync(string scriptId, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
            ValidateSearchInContent(scriptId, query, caseSensitive, isRegex);
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

        partial void ValidateSetAsyncCallStackDepth(int maxDepth);
        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        /// <param name = "maxDepth">Maximum depth of async call stacks. Setting to `0` will effectively disable collecting asynccall stacks (default).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAsyncCallStackDepthAsync(int maxDepth)
        {
            ValidateSetAsyncCallStackDepth(maxDepth);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxDepth", maxDepth);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setAsyncCallStackDepth", dict);
            return methodResult;
        }

        partial void ValidateSetBlackboxPatterns(string[] patterns);
        /// <summary>
        /// Replace previous blackbox patterns with passed ones. Forces backend to skip stepping/pausing in
        /// scripts with url matching one of the patterns. VM will try to leave blackboxed script by
        /// performing 'step in' several times, finally resorting to 'step out' if unsuccessful.
        /// </summary>
        /// <param name = "patterns">Array of regexps that will be used to check script url for blackbox state.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlackboxPatternsAsync(string[] patterns)
        {
            ValidateSetBlackboxPatterns(patterns);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("patterns", patterns);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBlackboxPatterns", dict);
            return methodResult;
        }

        partial void ValidateSetBlackboxedRanges(string scriptId, System.Collections.Generic.IList<CefSharp.DevTools.Debugger.ScriptPosition> positions);
        /// <summary>
        /// Makes backend skip steps in the script in blackboxed ranges. VM will try leave blacklisted
        /// scripts by performing 'step in' several times, finally resorting to 'step out' if unsuccessful.
        /// Positions array contains positions where blackbox state is changed. First interval isn't
        /// blackboxed. Array should be sorted.
        /// </summary>
        /// <param name = "scriptId">Id of the script.</param>
        /// <param name = "positions">positions</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBlackboxedRangesAsync(string scriptId, System.Collections.Generic.IList<CefSharp.DevTools.Debugger.ScriptPosition> positions)
        {
            ValidateSetBlackboxedRanges(scriptId, positions);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            dict.Add("positions", positions.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBlackboxedRanges", dict);
            return methodResult;
        }

        partial void ValidateSetBreakpoint(CefSharp.DevTools.Debugger.Location location, string condition = null);
        /// <summary>
        /// Sets JavaScript breakpoint at a given location.
        /// </summary>
        /// <param name = "location">Location to set breakpoint in.</param>
        /// <param name = "condition">Expression to use as a breakpoint condition. When specified, debugger will only stop on thebreakpoint if this expression evaluates to true.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetBreakpointResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetBreakpointResponse> SetBreakpointAsync(CefSharp.DevTools.Debugger.Location location, string condition = null)
        {
            ValidateSetBreakpoint(location, condition);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("location", location.ToDictionary());
            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpoint", dict);
            return methodResult.DeserializeJson<SetBreakpointResponse>();
        }

        partial void ValidateSetInstrumentationBreakpoint(string instrumentation);
        /// <summary>
        /// Sets instrumentation breakpoint.
        /// </summary>
        /// <param name = "instrumentation">Instrumentation name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetInstrumentationBreakpointResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetInstrumentationBreakpointResponse> SetInstrumentationBreakpointAsync(string instrumentation)
        {
            ValidateSetInstrumentationBreakpoint(instrumentation);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("instrumentation", instrumentation);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setInstrumentationBreakpoint", dict);
            return methodResult.DeserializeJson<SetInstrumentationBreakpointResponse>();
        }

        partial void ValidateSetBreakpointByUrl(int lineNumber, string url = null, string urlRegex = null, string scriptHash = null, int? columnNumber = null, string condition = null);
        /// <summary>
        /// Sets JavaScript breakpoint at given location specified either by URL or URL regex. Once this
        /// command is issued, all existing parsed scripts will have breakpoints resolved and returned in
        /// `locations` property. Further matching script parsing will result in subsequent
        /// `breakpointResolved` events issued. This logical breakpoint will survive page reloads.
        /// </summary>
        /// <param name = "lineNumber">Line number to set breakpoint at.</param>
        /// <param name = "url">URL of the resources to set breakpoint on.</param>
        /// <param name = "urlRegex">Regex pattern for the URLs of the resources to set breakpoints on. Either `url` or`urlRegex` must be specified.</param>
        /// <param name = "scriptHash">Script hash of the resources to set breakpoint on.</param>
        /// <param name = "columnNumber">Offset in the line to set breakpoint at.</param>
        /// <param name = "condition">Expression to use as a breakpoint condition. When specified, debugger will only stop on thebreakpoint if this expression evaluates to true.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetBreakpointByUrlResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetBreakpointByUrlResponse> SetBreakpointByUrlAsync(int lineNumber, string url = null, string urlRegex = null, string scriptHash = null, int? columnNumber = null, string condition = null)
        {
            ValidateSetBreakpointByUrl(lineNumber, url, urlRegex, scriptHash, columnNumber, condition);
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

        partial void ValidateSetBreakpointOnFunctionCall(string objectId, string condition = null);
        /// <summary>
        /// Sets JavaScript breakpoint before each call to the given function.
        /// If another function was created from the same source as a given one,
        /// calling it will also trigger the breakpoint.
        /// </summary>
        /// <param name = "objectId">Function object id.</param>
        /// <param name = "condition">Expression to use as a breakpoint condition. When specified, debugger willstop on the breakpoint if this expression evaluates to true.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetBreakpointOnFunctionCallResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetBreakpointOnFunctionCallResponse> SetBreakpointOnFunctionCallAsync(string objectId, string condition = null)
        {
            ValidateSetBreakpointOnFunctionCall(objectId, condition);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            if (!(string.IsNullOrEmpty(condition)))
            {
                dict.Add("condition", condition);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointOnFunctionCall", dict);
            return methodResult.DeserializeJson<SetBreakpointOnFunctionCallResponse>();
        }

        partial void ValidateSetBreakpointsActive(bool active);
        /// <summary>
        /// Activates / deactivates all breakpoints on the page.
        /// </summary>
        /// <param name = "active">New value for breakpoints active state.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBreakpointsActiveAsync(bool active)
        {
            ValidateSetBreakpointsActive(active);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("active", active);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setBreakpointsActive", dict);
            return methodResult;
        }

        partial void ValidateSetPauseOnExceptions(string state);
        /// <summary>
        /// Defines pause on exceptions state. Can be set to stop on all exceptions, uncaught exceptions or
        /// no exceptions. Initial pause on exceptions state is `none`.
        /// </summary>
        /// <param name = "state">Pause on exceptions mode.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPauseOnExceptionsAsync(string state)
        {
            ValidateSetPauseOnExceptions(state);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("state", state);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setPauseOnExceptions", dict);
            return methodResult;
        }

        partial void ValidateSetReturnValue(CefSharp.DevTools.Runtime.CallArgument newValue);
        /// <summary>
        /// Changes return value in top frame. Available only at return break position.
        /// </summary>
        /// <param name = "newValue">New return value.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetReturnValueAsync(CefSharp.DevTools.Runtime.CallArgument newValue)
        {
            ValidateSetReturnValue(newValue);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("newValue", newValue.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setReturnValue", dict);
            return methodResult;
        }

        partial void ValidateSetScriptSource(string scriptId, string scriptSource, bool? dryRun = null);
        /// <summary>
        /// Edits JavaScript source live.
        /// </summary>
        /// <param name = "scriptId">Id of the script to edit.</param>
        /// <param name = "scriptSource">New content of the script.</param>
        /// <param name = "dryRun">If true the change will not actually be applied. Dry run may be used to get resultdescription without actually modifying the code.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetScriptSourceResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetScriptSourceResponse> SetScriptSourceAsync(string scriptId, string scriptSource, bool? dryRun = null)
        {
            ValidateSetScriptSource(scriptId, scriptSource, dryRun);
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

        partial void ValidateSetSkipAllPauses(bool skip);
        /// <summary>
        /// Makes page not interrupt on any pauses (breakpoint, exception, dom exception etc).
        /// </summary>
        /// <param name = "skip">New value for skip pauses state.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetSkipAllPausesAsync(bool skip)
        {
            ValidateSetSkipAllPauses(skip);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("skip", skip);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setSkipAllPauses", dict);
            return methodResult;
        }

        partial void ValidateSetVariableValue(int scopeNumber, string variableName, CefSharp.DevTools.Runtime.CallArgument newValue, string callFrameId);
        /// <summary>
        /// Changes value of variable in a callframe. Object-based scopes are not supported and must be
        /// mutated manually.
        /// </summary>
        /// <param name = "scopeNumber">0-based number of scope as was listed in scope chain. Only 'local', 'closure' and 'catch'scope types are allowed. Other scopes could be manipulated manually.</param>
        /// <param name = "variableName">Variable name.</param>
        /// <param name = "newValue">New variable value.</param>
        /// <param name = "callFrameId">Id of callframe that holds variable.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetVariableValueAsync(int scopeNumber, string variableName, CefSharp.DevTools.Runtime.CallArgument newValue, string callFrameId)
        {
            ValidateSetVariableValue(scopeNumber, variableName, newValue, callFrameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scopeNumber", scopeNumber);
            dict.Add("variableName", variableName);
            dict.Add("newValue", newValue.ToDictionary());
            dict.Add("callFrameId", callFrameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.setVariableValue", dict);
            return methodResult;
        }

        partial void ValidateStepInto(bool? breakOnAsyncCall = null);
        /// <summary>
        /// Steps into the function call.
        /// </summary>
        /// <param name = "breakOnAsyncCall">Debugger will pause on the execution of the first async task which was scheduledbefore next pause.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepIntoAsync(bool? breakOnAsyncCall = null)
        {
            ValidateStepInto(breakOnAsyncCall);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepOutAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOut", dict);
            return methodResult;
        }

        /// <summary>
        /// Steps over the statement.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StepOverAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Debugger.stepOver", dict);
            return methodResult;
        }
    }
}
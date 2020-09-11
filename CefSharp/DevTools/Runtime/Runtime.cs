// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    using System.Linq;

    /// <summary>
    /// Runtime domain exposes JavaScript runtime by means of remote evaluation and mirror objects.
    public partial class Runtime : DevToolsDomainBase
    {
        public Runtime(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Add handler to promise with given promise object id.
        /// </summary>
        public async System.Threading.Tasks.Task<AwaitPromiseResponse> AwaitPromiseAsync(string promiseObjectId, bool? returnByValue = null, bool? generatePreview = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("promiseObjectId", promiseObjectId);
            if (returnByValue.HasValue)
            {
                dict.Add("returnByValue", returnByValue.Value);
            }

            if (generatePreview.HasValue)
            {
                dict.Add("generatePreview", generatePreview.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.awaitPromise", dict);
            return methodResult.DeserializeJson<AwaitPromiseResponse>();
        }

        /// <summary>
        /// Calls function with given declaration on the given object. Object group of the result is
        public async System.Threading.Tasks.Task<CallFunctionOnResponse> CallFunctionOnAsync(string functionDeclaration, string objectId = null, System.Collections.Generic.IList<CefSharp.DevTools.Runtime.CallArgument> arguments = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, int? executionContextId = null, string objectGroup = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("functionDeclaration", functionDeclaration);
            if (!(string.IsNullOrEmpty(objectId)))
            {
                dict.Add("objectId", objectId);
            }

            if ((arguments) != (null))
            {
                dict.Add("arguments", arguments.Select(x => x.ToDictionary()));
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

            if (userGesture.HasValue)
            {
                dict.Add("userGesture", userGesture.Value);
            }

            if (awaitPromise.HasValue)
            {
                dict.Add("awaitPromise", awaitPromise.Value);
            }

            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.callFunctionOn", dict);
            return methodResult.DeserializeJson<CallFunctionOnResponse>();
        }

        /// <summary>
        /// Compiles expression.
        /// </summary>
        public async System.Threading.Tasks.Task<CompileScriptResponse> CompileScriptAsync(string expression, string sourceURL, bool persistScript, int? executionContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("expression", expression);
            dict.Add("sourceURL", sourceURL);
            dict.Add("persistScript", persistScript);
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.compileScript", dict);
            return methodResult.DeserializeJson<CompileScriptResponse>();
        }

        /// <summary>
        /// Disables reporting of execution contexts creation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Discards collected exceptions and console API calls.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DiscardConsoleEntriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.discardConsoleEntries", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables reporting of execution contexts creation by means of `executionContextCreated` event.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Evaluates expression on global object.
        /// </summary>
        public async System.Threading.Tasks.Task<EvaluateResponse> EvaluateAsync(string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, int? contextId = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, bool? throwOnSideEffect = null, long? timeout = null, bool? disableBreaks = null, bool? replMode = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
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

            if (contextId.HasValue)
            {
                dict.Add("contextId", contextId.Value);
            }

            if (returnByValue.HasValue)
            {
                dict.Add("returnByValue", returnByValue.Value);
            }

            if (generatePreview.HasValue)
            {
                dict.Add("generatePreview", generatePreview.Value);
            }

            if (userGesture.HasValue)
            {
                dict.Add("userGesture", userGesture.Value);
            }

            if (awaitPromise.HasValue)
            {
                dict.Add("awaitPromise", awaitPromise.Value);
            }

            if (throwOnSideEffect.HasValue)
            {
                dict.Add("throwOnSideEffect", throwOnSideEffect.Value);
            }

            if (timeout.HasValue)
            {
                dict.Add("timeout", timeout.Value);
            }

            if (disableBreaks.HasValue)
            {
                dict.Add("disableBreaks", disableBreaks.Value);
            }

            if (replMode.HasValue)
            {
                dict.Add("replMode", replMode.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.evaluate", dict);
            return methodResult.DeserializeJson<EvaluateResponse>();
        }

        /// <summary>
        /// Returns the isolate id.
        /// </summary>
        public async System.Threading.Tasks.Task<GetIsolateIdResponse> GetIsolateIdAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.getIsolateId", dict);
            return methodResult.DeserializeJson<GetIsolateIdResponse>();
        }

        /// <summary>
        /// Returns the JavaScript heap usage.
        public async System.Threading.Tasks.Task<GetHeapUsageResponse> GetHeapUsageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.getHeapUsage", dict);
            return methodResult.DeserializeJson<GetHeapUsageResponse>();
        }

        /// <summary>
        /// Returns properties of a given object. Object group of the result is inherited from the target
        public async System.Threading.Tasks.Task<GetPropertiesResponse> GetPropertiesAsync(string objectId, bool? ownProperties = null, bool? accessorPropertiesOnly = null, bool? generatePreview = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            if (ownProperties.HasValue)
            {
                dict.Add("ownProperties", ownProperties.Value);
            }

            if (accessorPropertiesOnly.HasValue)
            {
                dict.Add("accessorPropertiesOnly", accessorPropertiesOnly.Value);
            }

            if (generatePreview.HasValue)
            {
                dict.Add("generatePreview", generatePreview.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.getProperties", dict);
            return methodResult.DeserializeJson<GetPropertiesResponse>();
        }

        /// <summary>
        /// Returns all let, const and class variables from global scope.
        /// </summary>
        public async System.Threading.Tasks.Task<GlobalLexicalScopeNamesResponse> GlobalLexicalScopeNamesAsync(int? executionContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.globalLexicalScopeNames", dict);
            return methodResult.DeserializeJson<GlobalLexicalScopeNamesResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<QueryObjectsResponse> QueryObjectsAsync(string prototypeObjectId, string objectGroup = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("prototypeObjectId", prototypeObjectId);
            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.queryObjects", dict);
            return methodResult.DeserializeJson<QueryObjectsResponse>();
        }

        /// <summary>
        /// Releases remote object with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseObjectAsync(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.releaseObject", dict);
            return methodResult;
        }

        /// <summary>
        /// Releases all remote objects that belong to a given group.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseObjectGroupAsync(string objectGroup)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectGroup", objectGroup);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.releaseObjectGroup", dict);
            return methodResult;
        }

        /// <summary>
        /// Tells inspected instance to run if it was waiting for debugger to attach.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RunIfWaitingForDebuggerAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.runIfWaitingForDebugger", dict);
            return methodResult;
        }

        /// <summary>
        /// Runs script with given id in a given context.
        /// </summary>
        public async System.Threading.Tasks.Task<RunScriptResponse> RunScriptAsync(string scriptId, int? executionContextId = null, string objectGroup = null, bool? silent = null, bool? includeCommandLineAPI = null, bool? returnByValue = null, bool? generatePreview = null, bool? awaitPromise = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("scriptId", scriptId);
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            if (silent.HasValue)
            {
                dict.Add("silent", silent.Value);
            }

            if (includeCommandLineAPI.HasValue)
            {
                dict.Add("includeCommandLineAPI", includeCommandLineAPI.Value);
            }

            if (returnByValue.HasValue)
            {
                dict.Add("returnByValue", returnByValue.Value);
            }

            if (generatePreview.HasValue)
            {
                dict.Add("generatePreview", generatePreview.Value);
            }

            if (awaitPromise.HasValue)
            {
                dict.Add("awaitPromise", awaitPromise.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.runScript", dict);
            return methodResult.DeserializeJson<RunScriptResponse>();
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAsyncCallStackDepthAsync(int maxDepth)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxDepth", maxDepth);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setAsyncCallStackDepth", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCustomObjectFormatterEnabledAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setCustomObjectFormatterEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetMaxCallStackSizeToCaptureAsync(int size)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("size", size);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setMaxCallStackSizeToCapture", dict);
            return methodResult;
        }

        /// <summary>
        /// Terminate current or next JavaScript execution.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TerminateExecutionAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.terminateExecution", dict);
            return methodResult;
        }

        /// <summary>
        /// If executionContextId is empty, adds binding with the given name on the
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddBindingAsync(string name, int? executionContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.addBinding", dict);
            return methodResult;
        }

        /// <summary>
        /// This method does not remove binding function from global object but
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveBindingAsync(string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.removeBinding", dict);
            return methodResult;
        }
    }
}
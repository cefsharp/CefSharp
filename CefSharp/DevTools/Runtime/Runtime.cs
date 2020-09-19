// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    using System.Linq;

    /// <summary>
    /// Runtime domain exposes JavaScript runtime by means of remote evaluation and mirror objects.
    /// Evaluation results are returned as mirror object that expose object type, string representation
    /// and unique identifier that can be used for further object reference. Original objects are
    /// maintained in memory unless they are either explicitly released or are released along with the
    /// other objects in their object group.
    /// </summary>
    public partial class Runtime : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Runtime(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateAwaitPromise(string promiseObjectId, bool? returnByValue = null, bool? generatePreview = null);
        /// <summary>
        /// Add handler to promise with given promise object id.
        /// </summary>
        /// <param name = "promiseObjectId">Identifier of the promise.</param>
        /// <param name = "returnByValue">Whether the result is expected to be a JSON object that should be sent by value.</param>
        /// <param name = "generatePreview">Whether preview should be generated for the result.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;AwaitPromiseResponse&gt;</returns>
        public async System.Threading.Tasks.Task<AwaitPromiseResponse> AwaitPromiseAsync(string promiseObjectId, bool? returnByValue = null, bool? generatePreview = null)
        {
            ValidateAwaitPromise(promiseObjectId, returnByValue, generatePreview);
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

        partial void ValidateCallFunctionOn(string functionDeclaration, string objectId = null, System.Collections.Generic.IList<CefSharp.DevTools.Runtime.CallArgument> arguments = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, int? executionContextId = null, string objectGroup = null);
        /// <summary>
        /// Calls function with given declaration on the given object. Object group of the result is
        /// inherited from the target object.
        /// </summary>
        /// <param name = "functionDeclaration">Declaration of the function to call.</param>
        /// <param name = "objectId">Identifier of the object to call function on. Either objectId or executionContextId should
        public async System.Threading.Tasks.Task<CallFunctionOnResponse> CallFunctionOnAsync(string functionDeclaration, string objectId = null, System.Collections.Generic.IList<CefSharp.DevTools.Runtime.CallArgument> arguments = null, bool? silent = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, int? executionContextId = null, string objectGroup = null)
        {
            ValidateCallFunctionOn(functionDeclaration, objectId, arguments, silent, returnByValue, generatePreview, userGesture, awaitPromise, executionContextId, objectGroup);
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

        partial void ValidateCompileScript(string expression, string sourceURL, bool persistScript, int? executionContextId = null);
        /// <summary>
        /// Compiles expression.
        /// </summary>
        /// <param name = "expression">Expression to compile.</param>
        /// <param name = "sourceURL">Source url to be set for the script.</param>
        /// <param name = "persistScript">Specifies whether the compiled script should be persisted.</param>
        /// <param name = "executionContextId">Specifies in which execution context to perform script run. If the parameter is omitted the
        public async System.Threading.Tasks.Task<CompileScriptResponse> CompileScriptAsync(string expression, string sourceURL, bool persistScript, int? executionContextId = null)
        {
            ValidateCompileScript(expression, sourceURL, persistScript, executionContextId);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Discards collected exceptions and console API calls.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DiscardConsoleEntriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.discardConsoleEntries", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables reporting of execution contexts creation by means of `executionContextCreated` event.
        /// When the reporting gets enabled the event will be sent immediately for each existing execution
        /// context.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.enable", dict);
            return methodResult;
        }

        partial void ValidateEvaluate(string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, int? contextId = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, bool? throwOnSideEffect = null, long? timeout = null, bool? disableBreaks = null, bool? replMode = null, bool? allowUnsafeEvalBlockedByCSP = null);
        /// <summary>
        /// Evaluates expression on global object.
        /// </summary>
        /// <param name = "expression">Expression to evaluate.</param>
        /// <param name = "objectGroup">Symbolic group name that can be used to release multiple objects.</param>
        /// <param name = "includeCommandLineAPI">Determines whether Command Line API should be available during the evaluation.</param>
        /// <param name = "silent">In silent mode exceptions thrown during evaluation are not reported and do not pause
        public async System.Threading.Tasks.Task<EvaluateResponse> EvaluateAsync(string expression, string objectGroup = null, bool? includeCommandLineAPI = null, bool? silent = null, int? contextId = null, bool? returnByValue = null, bool? generatePreview = null, bool? userGesture = null, bool? awaitPromise = null, bool? throwOnSideEffect = null, long? timeout = null, bool? disableBreaks = null, bool? replMode = null, bool? allowUnsafeEvalBlockedByCSP = null)
        {
            ValidateEvaluate(expression, objectGroup, includeCommandLineAPI, silent, contextId, returnByValue, generatePreview, userGesture, awaitPromise, throwOnSideEffect, timeout, disableBreaks, replMode, allowUnsafeEvalBlockedByCSP);
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

            if (allowUnsafeEvalBlockedByCSP.HasValue)
            {
                dict.Add("allowUnsafeEvalBlockedByCSP", allowUnsafeEvalBlockedByCSP.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.evaluate", dict);
            return methodResult.DeserializeJson<EvaluateResponse>();
        }

        /// <summary>
        /// Returns the isolate id.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetIsolateIdResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetIsolateIdResponse> GetIsolateIdAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.getIsolateId", dict);
            return methodResult.DeserializeJson<GetIsolateIdResponse>();
        }

        /// <summary>
        /// Returns the JavaScript heap usage.
        /// It is the total usage of the corresponding isolate not scoped to a particular Runtime.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetHeapUsageResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetHeapUsageResponse> GetHeapUsageAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.getHeapUsage", dict);
            return methodResult.DeserializeJson<GetHeapUsageResponse>();
        }

        partial void ValidateGetProperties(string objectId, bool? ownProperties = null, bool? accessorPropertiesOnly = null, bool? generatePreview = null);
        /// <summary>
        /// Returns properties of a given object. Object group of the result is inherited from the target
        /// object.
        /// </summary>
        /// <param name = "objectId">Identifier of the object to return properties for.</param>
        /// <param name = "ownProperties">If true, returns properties belonging only to the element itself, not to its prototype
        public async System.Threading.Tasks.Task<GetPropertiesResponse> GetPropertiesAsync(string objectId, bool? ownProperties = null, bool? accessorPropertiesOnly = null, bool? generatePreview = null)
        {
            ValidateGetProperties(objectId, ownProperties, accessorPropertiesOnly, generatePreview);
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

        partial void ValidateGlobalLexicalScopeNames(int? executionContextId = null);
        /// <summary>
        /// Returns all let, const and class variables from global scope.
        /// </summary>
        /// <param name = "executionContextId">Specifies in which execution context to lookup global scope variables.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GlobalLexicalScopeNamesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GlobalLexicalScopeNamesResponse> GlobalLexicalScopeNamesAsync(int? executionContextId = null)
        {
            ValidateGlobalLexicalScopeNames(executionContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.globalLexicalScopeNames", dict);
            return methodResult.DeserializeJson<GlobalLexicalScopeNamesResponse>();
        }

        partial void ValidateQueryObjects(string prototypeObjectId, string objectGroup = null);
        /// <summary>
        /// QueryObjects
        /// </summary>
        /// <param name = "prototypeObjectId">Identifier of the prototype to return objects for.</param>
        /// <param name = "objectGroup">Symbolic group name that can be used to release the results.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;QueryObjectsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<QueryObjectsResponse> QueryObjectsAsync(string prototypeObjectId, string objectGroup = null)
        {
            ValidateQueryObjects(prototypeObjectId, objectGroup);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("prototypeObjectId", prototypeObjectId);
            if (!(string.IsNullOrEmpty(objectGroup)))
            {
                dict.Add("objectGroup", objectGroup);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.queryObjects", dict);
            return methodResult.DeserializeJson<QueryObjectsResponse>();
        }

        partial void ValidateReleaseObject(string objectId);
        /// <summary>
        /// Releases remote object with given id.
        /// </summary>
        /// <param name = "objectId">Identifier of the object to release.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseObjectAsync(string objectId)
        {
            ValidateReleaseObject(objectId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectId", objectId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.releaseObject", dict);
            return methodResult;
        }

        partial void ValidateReleaseObjectGroup(string objectGroup);
        /// <summary>
        /// Releases all remote objects that belong to a given group.
        /// </summary>
        /// <param name = "objectGroup">Symbolic object group name.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReleaseObjectGroupAsync(string objectGroup)
        {
            ValidateReleaseObjectGroup(objectGroup);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("objectGroup", objectGroup);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.releaseObjectGroup", dict);
            return methodResult;
        }

        /// <summary>
        /// Tells inspected instance to run if it was waiting for debugger to attach.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RunIfWaitingForDebuggerAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.runIfWaitingForDebugger", dict);
            return methodResult;
        }

        partial void ValidateRunScript(string scriptId, int? executionContextId = null, string objectGroup = null, bool? silent = null, bool? includeCommandLineAPI = null, bool? returnByValue = null, bool? generatePreview = null, bool? awaitPromise = null);
        /// <summary>
        /// Runs script with given id in a given context.
        /// </summary>
        /// <param name = "scriptId">Id of the script to run.</param>
        /// <param name = "executionContextId">Specifies in which execution context to perform script run. If the parameter is omitted the
        public async System.Threading.Tasks.Task<RunScriptResponse> RunScriptAsync(string scriptId, int? executionContextId = null, string objectGroup = null, bool? silent = null, bool? includeCommandLineAPI = null, bool? returnByValue = null, bool? generatePreview = null, bool? awaitPromise = null)
        {
            ValidateRunScript(scriptId, executionContextId, objectGroup, silent, includeCommandLineAPI, returnByValue, generatePreview, awaitPromise);
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

        partial void ValidateSetAsyncCallStackDepth(int maxDepth);
        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        /// <param name = "maxDepth">Maximum depth of async call stacks. Setting to `0` will effectively disable collecting async
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAsyncCallStackDepthAsync(int maxDepth)
        {
            ValidateSetAsyncCallStackDepth(maxDepth);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("maxDepth", maxDepth);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setAsyncCallStackDepth", dict);
            return methodResult;
        }

        partial void ValidateSetCustomObjectFormatterEnabled(bool enabled);
        /// <summary>
        /// SetCustomObjectFormatterEnabled
        /// </summary>
        /// <param name = "enabled">enabled</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCustomObjectFormatterEnabledAsync(bool enabled)
        {
            ValidateSetCustomObjectFormatterEnabled(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setCustomObjectFormatterEnabled", dict);
            return methodResult;
        }

        partial void ValidateSetMaxCallStackSizeToCapture(int size);
        /// <summary>
        /// SetMaxCallStackSizeToCapture
        /// </summary>
        /// <param name = "size">size</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetMaxCallStackSizeToCaptureAsync(int size)
        {
            ValidateSetMaxCallStackSizeToCapture(size);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("size", size);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.setMaxCallStackSizeToCapture", dict);
            return methodResult;
        }

        /// <summary>
        /// Terminate current or next JavaScript execution.
        /// Will cancel the termination when the outer-most script execution ends.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> TerminateExecutionAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.terminateExecution", dict);
            return methodResult;
        }

        partial void ValidateAddBinding(string name, int? executionContextId = null);
        /// <summary>
        /// If executionContextId is empty, adds binding with the given name on the
        /// global objects of all inspected contexts, including those created later,
        /// bindings survive reloads.
        /// If executionContextId is specified, adds binding only on global object of
        /// given execution context.
        /// Binding function takes exactly one argument, this argument should be string,
        /// in case of any other input, function throws an exception.
        /// Each binding function call produces Runtime.bindingCalled notification.
        /// </summary>
        /// <param name = "name">name</param>
        /// <param name = "executionContextId">executionContextId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddBindingAsync(string name, int? executionContextId = null)
        {
            ValidateAddBinding(name, executionContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            if (executionContextId.HasValue)
            {
                dict.Add("executionContextId", executionContextId.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.addBinding", dict);
            return methodResult;
        }

        partial void ValidateRemoveBinding(string name);
        /// <summary>
        /// This method does not remove binding function from global object but
        /// unsubscribes current runtime agent from Runtime.bindingCalled notifications.
        /// </summary>
        /// <param name = "name">name</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveBindingAsync(string name)
        {
            ValidateRemoveBinding(name);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Runtime.removeBinding", dict);
            return methodResult;
        }
    }
}
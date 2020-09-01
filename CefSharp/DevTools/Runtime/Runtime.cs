// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Runtime domain exposes JavaScript runtime by means of remote evaluation and mirror objects.
    public partial class Runtime
    {
        public Runtime(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Add handler to promise with given promise object id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AwaitPromise(string promiseObjectId, bool returnByValue, bool generatePreview)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"promiseObjectId", promiseObjectId}, {"returnByValue", returnByValue}, {"generatePreview", generatePreview}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.AwaitPromise", dict);
            return result;
        }

        /// <summary>
        /// Calls function with given declaration on the given object. Object group of the result is
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CallFunctionOn(string functionDeclaration, string objectId, System.Collections.Generic.IList<CallArgument> arguments, bool silent, bool returnByValue, bool generatePreview, bool userGesture, bool awaitPromise, int executionContextId, string objectGroup)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"functionDeclaration", functionDeclaration}, {"objectId", objectId}, {"arguments", arguments}, {"silent", silent}, {"returnByValue", returnByValue}, {"generatePreview", generatePreview}, {"userGesture", userGesture}, {"awaitPromise", awaitPromise}, {"executionContextId", executionContextId}, {"objectGroup", objectGroup}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.CallFunctionOn", dict);
            return result;
        }

        /// <summary>
        /// Compiles expression.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CompileScript(string expression, string sourceURL, bool persistScript, int executionContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"expression", expression}, {"sourceURL", sourceURL}, {"persistScript", persistScript}, {"executionContextId", executionContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.CompileScript", dict);
            return result;
        }

        /// <summary>
        /// Disables reporting of execution contexts creation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.Disable", dict);
            return result;
        }

        /// <summary>
        /// Discards collected exceptions and console API calls.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DiscardConsoleEntries()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.DiscardConsoleEntries", dict);
            return result;
        }

        /// <summary>
        /// Enables reporting of execution contexts creation by means of `executionContextCreated` event.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.Enable", dict);
            return result;
        }

        /// <summary>
        /// Evaluates expression on global object.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Evaluate(string expression, string objectGroup, bool includeCommandLineAPI, bool silent, int contextId, bool returnByValue, bool generatePreview, bool userGesture, bool awaitPromise, bool throwOnSideEffect, long timeout, bool disableBreaks, bool replMode, bool allowUnsafeEvalBlockedByCSP)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"expression", expression}, {"objectGroup", objectGroup}, {"includeCommandLineAPI", includeCommandLineAPI}, {"silent", silent}, {"contextId", contextId}, {"returnByValue", returnByValue}, {"generatePreview", generatePreview}, {"userGesture", userGesture}, {"awaitPromise", awaitPromise}, {"throwOnSideEffect", throwOnSideEffect}, {"timeout", timeout}, {"disableBreaks", disableBreaks}, {"replMode", replMode}, {"allowUnsafeEvalBlockedByCSP", allowUnsafeEvalBlockedByCSP}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.Evaluate", dict);
            return result;
        }

        /// <summary>
        /// Returns the isolate id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetIsolateId()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.GetIsolateId", dict);
            return result;
        }

        /// <summary>
        /// Returns the JavaScript heap usage.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetHeapUsage()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.GetHeapUsage", dict);
            return result;
        }

        /// <summary>
        /// Returns properties of a given object. Object group of the result is inherited from the target
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetProperties(string objectId, bool ownProperties, bool accessorPropertiesOnly, bool generatePreview)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, {"ownProperties", ownProperties}, {"accessorPropertiesOnly", accessorPropertiesOnly}, {"generatePreview", generatePreview}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.GetProperties", dict);
            return result;
        }

        /// <summary>
        /// Returns all let, const and class variables from global scope.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GlobalLexicalScopeNames(int executionContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"executionContextId", executionContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.GlobalLexicalScopeNames", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> QueryObjects(string prototypeObjectId, string objectGroup)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"prototypeObjectId", prototypeObjectId}, {"objectGroup", objectGroup}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.QueryObjects", dict);
            return result;
        }

        /// <summary>
        /// Releases remote object with given id.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ReleaseObject(string objectId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectId", objectId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.ReleaseObject", dict);
            return result;
        }

        /// <summary>
        /// Releases all remote objects that belong to a given group.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ReleaseObjectGroup(string objectGroup)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"objectGroup", objectGroup}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.ReleaseObjectGroup", dict);
            return result;
        }

        /// <summary>
        /// Tells inspected instance to run if it was waiting for debugger to attach.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RunIfWaitingForDebugger()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.RunIfWaitingForDebugger", dict);
            return result;
        }

        /// <summary>
        /// Runs script with given id in a given context.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RunScript(string scriptId, int executionContextId, string objectGroup, bool silent, bool includeCommandLineAPI, bool returnByValue, bool generatePreview, bool awaitPromise)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptId", scriptId}, {"executionContextId", executionContextId}, {"objectGroup", objectGroup}, {"silent", silent}, {"includeCommandLineAPI", includeCommandLineAPI}, {"returnByValue", returnByValue}, {"generatePreview", generatePreview}, {"awaitPromise", awaitPromise}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.RunScript", dict);
            return result;
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAsyncCallStackDepth(int maxDepth)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"maxDepth", maxDepth}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.SetAsyncCallStackDepth", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetCustomObjectFormatterEnabled(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.SetCustomObjectFormatterEnabled", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetMaxCallStackSizeToCapture(int size)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"size", size}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.SetMaxCallStackSizeToCapture", dict);
            return result;
        }

        /// <summary>
        /// Terminate current or next JavaScript execution.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> TerminateExecution()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.TerminateExecution", dict);
            return result;
        }

        /// <summary>
        /// If executionContextId is empty, adds binding with the given name on the
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AddBinding(string name, int executionContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"name", name}, {"executionContextId", executionContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.AddBinding", dict);
            return result;
        }

        /// <summary>
        /// This method does not remove binding function from global object but
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveBinding(string name)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"name", name}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Runtime.RemoveBinding", dict);
            return result;
        }
    }
}
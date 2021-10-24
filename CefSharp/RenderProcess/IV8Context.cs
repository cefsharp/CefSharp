// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.RenderProcess
{
    /// <summary>
    /// V8 context handle.
    /// V8 handles can only be accessed from the thread on which they are created.
    /// Valid threads for creating a V8 handle include the render process main thread (TID_RENDERER) and WebWorker threads.
    /// A task runner for posting tasks on the associated thread can be retrieved via the CefV8Context::GetTaskRunner() method.
    /// </summary>
    /// <remarks>
    /// V8 is Google’s open source high-performance JavaScript and WebAssembly engine.
    /// </remarks>
    public interface IV8Context
    {
        /// <summary>
        /// Execute a string of JavaScript code in this V8 context.
        /// </summary>
        /// <param name="code">JavaScript code to execute</param>
        /// <param name="scriptUrl">Is the URL where the script in question can be found, if any</param>
        /// <param name="startLine">Is the base line number to use for error reporting.</param>
        /// <param name="exception">Is the exception if any.</param>
        /// <returns>On success the function will return true. On failure <paramref name="exception"/> will be set to the exception, if any, and the function will return false.</returns>
        bool Execute(string code, string scriptUrl, int startLine, out V8Exception exception);
    }
}

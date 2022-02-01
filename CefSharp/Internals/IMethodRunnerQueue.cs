// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Run (execute) calls made from Javascript to .Net methods
    /// </summary>
    public interface IMethodRunnerQueue : IDisposable
    {
        /// <summary>
        /// Method invocation was completed.
        /// </summary>
        event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        /// <summary>
        /// Enqueue a method invocation
        /// </summary>
        /// <param name="methodInvocation">method invocation</param>
        void Enqueue(MethodInvocation methodInvocation);
    }
}

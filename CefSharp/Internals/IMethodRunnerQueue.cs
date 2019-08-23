// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public interface IMethodRunnerQueue : IDisposable
    {
        event EventHandler<MethodInvocationCompleteArgs> MethodInvocationComplete;

        void Enqueue(MethodInvocation methodInvocation);
    }
}

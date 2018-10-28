// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using CefSharp.ModelBinding;

namespace CefSharp.Example.ModelBinding
{
    public class MethodInterceptorLogger : IMethodInterceptor
    {
        object IMethodInterceptor.Intercept(Func<object> method, string methodName)
        {
            object result = method();
            Debug.WriteLine("Called " + methodName);
            return result;
        }
    }
}

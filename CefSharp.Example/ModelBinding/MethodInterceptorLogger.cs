// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.ModelBinding;

namespace CefSharp.Example.ModelBinding
{
    public class MethodInterceptorLogger : IMethodInterceptor
    {
        public bool IsAsync => false;

        object IMethodInterceptor.Intercept(Func<object[], object> method, object[] parameters, string methodName)
        {
            var result = method(parameters);
            Debug.WriteLine("Called " + methodName);
            return result;
        }

        public Task<object> InterceptAsync(Func<object[], object> method, object[] parameters, string methodName)
        {
            throw new NotImplementedException();
        }
    }
}

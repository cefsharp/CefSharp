// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.ModelBinding;

namespace CefSharp.Example.ModelBinding
{
    public class MethodInterceptorLogger : IAsyncMethodInterceptor
    {
        object IMethodInterceptor.Intercept(Func<object[], object> method, object[] parameters, string methodName)
        {
            Debug.WriteLine("Before Method Call:" + methodName);
            var result = method(parameters);
            Debug.WriteLine("After Method Call:" + methodName);
            return result;
        }

        async Task<object> IAsyncMethodInterceptor.InterceptAsync(Func<object[], object> method, object[] parameters, string methodName)
        {
            var result = method(parameters);

            if(result == null)
            {
                return null;
            }

            //Let the CLR workout if it's a generic task or not.
            return await InterceptAsync((dynamic)result, methodName);
        }

        private static async Task<object> InterceptAsync(Task task, string methodName)
        {
            Debug.WriteLine("Before Method Call:" + methodName);

            await task.ConfigureAwait(false);
            // do the logging here, as continuation work for Task...
            Debug.WriteLine("After Method Call:" + methodName);

            //As our task doesn't return a result we return null.
            //We currently don't support an equivilent of undefined
            //in javascript
            return null;
        }

        private static async Task<T> InterceptAsync<T>(Task<T> task, string methodName)
        {
            Debug.WriteLine("Before Method Call:" + methodName);
            T result = await task.ConfigureAwait(false);
            Debug.WriteLine("After Method Call:" + methodName);

            // do the logging here, as continuation work for Task<T>...
            return result;
        }
    }
}

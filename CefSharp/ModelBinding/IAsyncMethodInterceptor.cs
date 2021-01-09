// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability intercept async/sync Net method calls made from javascript as part of the
    /// JavascriptBinding (JSB) implementation. One example use case is logging method calls.
    /// Extends <see cref="IMethodInterceptor"/> to add async support.
    /// </summary>
    public interface IAsyncMethodInterceptor : IMethodInterceptor
    {
        /// <summary>
        /// Called before an async method is invoked. You are now responsible for evaluating
        /// the function and returning the result. Only methods that return a <see cref="Task"/>
        /// will call this method, other non asynchronous types will call
        /// <see cref="IMethodInterceptor.Intercept(Func{object[], object}, object[], string)"/>.
        /// (async void method will also call Intercept as they do not return a Task).
        /// </summary>
        /// <param name="method">A Func that represents the method to be called</param>
        /// <param name="parameters">paramaters to be passed to <paramref name="method"/></param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <returns>A Task representing the method result</returns>
        /// <example>
        /// Task&lt;object&gt IAsyncMethodInterceptor.InterceptAsync(Func&lt;object[], object&gt; method, object[] parameters, string methodName)
        /// {
        ///   object result = method(parameters);
        ///   Debug.WriteLine("Called " + methodName);
        ///   return result;
        ///  }
        /// </example>
        Task<object> InterceptAsync(Func<object[], object> method, object[] parameters, string methodName);
    }
}

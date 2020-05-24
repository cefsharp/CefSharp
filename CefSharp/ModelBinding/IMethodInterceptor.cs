// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability intercept Net method calls made from javascript as part of the
    /// JavascriptBinding (JSB) implementation. One example use case is logging method calls.
    /// </summary>
    public interface IMethodInterceptor
    {
        /// <summary>
        /// To avoid forcing users to use <see cref="Task.GetAwaiter()"/> for interception calls, this call informs the <see cref="IJavascriptObjectRepository"/> to use <see cref="InterceptAsync"/>
        /// </summary>
        bool IsAsynchronous { get; }

        /// <summary>
        /// When <see cref="IMethodInterceptor.IsAsynchronous"/> is set to true, this is called before the method is invoked. You are now responsible for evaluating
        /// the function and returning the result.
        /// </summary>
        /// <param name="method">A Func that represents the method to be called</param>
        /// <param name="parameters">parameters to be passed to <paramref name="method"/></param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <returns>The method result</returns>
        /// <example>
        /// <see cref="TypeSafeInterceptor"/>
        /// </example>
        Task<object> InterceptAsync(Func<object[], object> method, object[] parameters, string methodName);

        /// <summary>
        /// Called before the method is invokved. You are now responsible for evaluating
        /// the function and returning the result.
        /// </summary>
        /// <param name="method">A Func that represents the method to be called</param>
        /// <param name="parameters">paramaters to be passed to <paramref name="method"/></param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <returns>The method result</returns>
        /// <example>
        /// 
        /// object IMethodInterceptor.Intercept(Func&lt;object[], object&gt; method, object[] parameters, string methodName)
        /// {
        ///   object result = method(parameters);
        ///   Debug.WriteLine("Called " + methodName);
        ///   return result;
        ///  }
        /// </example>
        object Intercept(Func<object[], object> method, object[] parameters, string methodName);
    }
}

﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability intercept Net method calls made from javascript as part of the
    /// JavascriptBinding (JSB) implementation. One example use case is logging method calls.
    public interface IMethodInterceptor
    {
        /// <summary>
        /// Called before the method is invokved. You are now responsible for evaluating
        /// the function and returning the result.
        /// </summary>
        /// <param name="method">A Func that represents the method to be called</param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <returns>The method result</returns>
        /// <example>
        /// object IMethodInterceptor.Intercept(Func&lt;object&gt; method, string methodName)
        /// {
        ///   object result = method();
        ///   Debug.WriteLine("Called " + methodName);
        ///   return result;
        ///  }
        /// </example>
        object Intercept(Func<object> method, string methodName);
    }
}
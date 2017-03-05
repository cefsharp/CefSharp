// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Linq.Expressions;

namespace CefSharp.Internals
{
    internal static class ReflectionUtils
    {
        public static string GetMethodName<T>(Expression<Func<T, object>> expression)
        {
            var methodCallExpression = (MethodCallExpression)expression.Body;
            return methodCallExpression.Method.Name;
        }
    }
}

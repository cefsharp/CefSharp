// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// Interface used to break reference cycles in CefSharp.Core C++ code.
    /// This will ALWAYS be a ManagedCefBrowserAdapter instance.
    /// </summary>
    public interface IBrowserAdapter
    {
        Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout);
        Task<JavascriptResponse> EvaluateScriptAsync(int browserId, Int64 frameId, string script, TimeSpan? timeout);
    }
}

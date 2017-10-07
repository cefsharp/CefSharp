// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Generic callback interface used for asynchronous completion. 
    /// </summary>
    public interface ICompletionCallback : IDisposable
    {
        /// <summary>
        /// Method that will be called once the task is complete. 
        /// </summary>
        void OnComplete();

        /// <summary>
        /// Returns a Task that was used to set the result of the CookieCallback 
        /// </summary>
        Task SetResultTask { get; }
    }
}

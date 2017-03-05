// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// A <see cref="IStringVisitor"/> that uses a TaskCompletionSource
    /// to simplify things
    /// </summary>
    public class TaskStringVisitor : IStringVisitor
    {
        private readonly TaskCompletionSource<string> taskCompletionSource;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskStringVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<string>();
        }

        /// <summary>
        ///  Method that will be executed.
        /// </summary>
        /// <param name="str">string (result of async execution)</param>
        void IStringVisitor.Visit(string str)
        {
            taskCompletionSource.TrySetResultAsync(str);
        }

        /// <summary>
        /// Task that can be awaited for the result to be retrieved async
        /// </summary>
        public Task<string> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            
        }
    }
}

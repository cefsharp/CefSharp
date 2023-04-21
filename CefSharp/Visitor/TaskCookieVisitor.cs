// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// Cookie Visitor implementation that uses a TaskCompletionSource
    /// to return a List of cookies
    /// </summary>
    public class TaskCookieVisitor : ICookieVisitor
    {
        private readonly TaskCompletionSource<List<Cookie>> taskCompletionSource;
        private List<Cookie> list;

        /// <summary>
        /// Initializes a new instance of the TaskCookieVisitor class.
        /// </summary>
        public TaskCookieVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<List<Cookie>>(TaskCreationOptions.RunContinuationsAsynchronously);
            list = new List<Cookie>();
        }

        /// <inheritdoc/>
        bool ICookieVisitor.Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            list.Add(cookie);

            if (count == (total - 1))
            {
                taskCompletionSource.TrySetResult(list);
            }

            return true;
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            if (list != null && list.Count == 0)
            {
                taskCompletionSource.TrySetResult(list);
            }

            list = null;
        }

        /// <summary>
        /// Task that can be awaited for the result to be retrieved async
        /// </summary>
        public Task<List<Cookie>> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}

// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

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
        /// Default constructor
        /// </summary>
        public TaskCookieVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<List<Cookie>>();
            list = new List<Cookie>();
        }

        bool ICookieVisitor.Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            list.Add(cookie);            

            if(count == (total - 1))
            {
                //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                taskCompletionSource.TrySetResultAsync(list);
            }

            return true;
        }

        void IDisposable.Dispose()
        {
            if(list != null && list.Count == 0)
            {
                //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                taskCompletionSource.TrySetResultAsync(list);
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

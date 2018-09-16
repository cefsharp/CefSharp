// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// A <see cref="INavigationEntryVisitor"/> that uses a TaskCompletionSource
    /// to simplify things
    /// </summary>
    public class TaskNavigationEntryVisitor : INavigationEntryVisitor
    {
        private TaskCompletionSource<List<NavigationEntry>> taskCompletionSource;
        private List<NavigationEntry> list;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskNavigationEntryVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<List<NavigationEntry>>();
            list = new List<NavigationEntry>();
        }

        bool INavigationEntryVisitor.Visit(NavigationEntry entry, bool current, int index, int total)
        {
            list.Add(entry);

            return true;
        }

        void IDisposable.Dispose()
        {
            if (list != null)
            {
                //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                taskCompletionSource.TrySetResultAsync(list);
            }

            list = null;
            taskCompletionSource = null;
        }

        /// <summary>
        /// Task that can be awaited for the result to be retrieved async
        /// </summary>
        public Task<List<NavigationEntry>> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}

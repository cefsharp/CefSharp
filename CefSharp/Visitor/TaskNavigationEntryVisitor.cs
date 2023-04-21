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
    /// A <see cref="INavigationEntryVisitor"/> implementation that uses a <see cref="TaskCompletionSource{TResult}"/>
    /// that allows you to call await/ContinueWith to get the list of NavigationEntries
    /// </summary>
    public class TaskNavigationEntryVisitor : INavigationEntryVisitor
    {
        private TaskCompletionSource<List<NavigationEntry>> taskCompletionSource;
        private List<NavigationEntry> list;

        /// <summary>
        /// Initializes a new instance of the TaskNavigationEntryVisitor class.
        /// </summary>
        public TaskNavigationEntryVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<List<NavigationEntry>>(TaskCreationOptions.RunContinuationsAsynchronously);
            list = new List<NavigationEntry>();
        }

        /// <inheritdoc/>
        bool INavigationEntryVisitor.Visit(NavigationEntry entry, bool current, int index, int total)
        {
            list.Add(entry);

            return true;
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            if (list != null)
            {
                taskCompletionSource.TrySetResult(list);
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

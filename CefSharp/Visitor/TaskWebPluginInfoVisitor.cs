// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskWebPluginInfoVisitor : IWebPluginInfoVisitor
    {
        private TaskCompletionSource<List<WebPluginInfo>> taskCompletionSource;
        private List<WebPluginInfo> list;

        public TaskWebPluginInfoVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<List<WebPluginInfo>>();
            list = new List<WebPluginInfo>();
        }

        bool IWebPluginInfoVisitor.Visit(WebPluginInfo plugin, int count, int total)
        {
            list.Add(plugin);

            //Return true to keep visiting plugins
            return true;
        }

        /// <summary>
        /// Task that can be awaited for the result to be retrieved async
        /// </summary>
        public Task<List<WebPluginInfo>> Task
        {
            get { return taskCompletionSource.Task; }
        }

        void IDisposable.Dispose()
        {
            //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
            taskCompletionSource.TrySetResultAsync(list);

            list = null;
            taskCompletionSource = null;
        }
    }
}

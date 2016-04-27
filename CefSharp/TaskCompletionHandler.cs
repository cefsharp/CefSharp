// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public class TaskCompletionHandler : ICompletionCallback
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;

        public TaskCompletionHandler()
        {
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        public void OnComplete()
        {
            taskCompletionSource.TrySetResultAsync(true);
        }

        public Task<bool> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}

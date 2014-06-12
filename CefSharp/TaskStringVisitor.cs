// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;

namespace CefSharp
{
    public class TaskStringVisitor : IStringVisitor
    {
        private readonly TaskCompletionSource<string> taskCompletionSource;

        public TaskStringVisitor()
        {
            taskCompletionSource = new TaskCompletionSource<string>();
        }

        public void Visit(string str)
        {
            taskCompletionSource.SetResult(str);
        }

        public Task<string> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}

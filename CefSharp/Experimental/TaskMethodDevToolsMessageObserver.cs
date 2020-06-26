// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Callback;
using CefSharp.Internals;

namespace CefSharp.Experimental
{
    /// <summary>
    /// For capturing the response from a DevTools Method
    /// (Doesn't handle DevTools events)
    /// </summary>
    public class TaskMethodDevToolsMessageObserver : IDevToolsMessageObserver
    {
        private readonly TaskCompletionSource<Tuple<bool, byte[]>> taskCompletionSource = new TaskCompletionSource<Tuple<bool, byte[]>>();
        private readonly int matchMessageId;

        public TaskMethodDevToolsMessageObserver(int messageId)
        {
            matchMessageId = messageId;
        }

        void IDisposable.Dispose()
        {

        }

        void IDevToolsMessageObserver.OnDevToolsAgentAttached(IBrowser browser)
        {

        }

        void IDevToolsMessageObserver.OnDevToolsAgentDetached(IBrowser browser)
        {

        }

        void IDevToolsMessageObserver.OnDevToolsEvent(IBrowser browser, string method, Stream parameters)
        {

        }

        bool IDevToolsMessageObserver.OnDevToolsMessage(IBrowser browser, Stream message)
        {
            return false;
        }

        void IDevToolsMessageObserver.OnDevToolsMethodResult(IBrowser browser, int messageId, bool success, Stream result)
        {
            //We found the message Id we're after
            if (matchMessageId == messageId)
            {
                var memoryStream = new MemoryStream((int)result.Length);

                result.CopyTo(memoryStream);

                var response = Tuple.Create(success, memoryStream.ToArray());

                taskCompletionSource.TrySetResultAsync(response);
            }
        }

        public Task<Tuple<bool, byte[]>> Task
        {
            get { return taskCompletionSource.Task; }
        }
    }
}

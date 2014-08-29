// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp
{
    public abstract class CefBrowserBase : DisposableResource
    {
        public int BrowserId { get; set; }
        public TaskFactory RenderThreadTaskFactory { get; protected set; }

        public Task<object> EvaluateScript(long frameId, string script, TimeSpan timeout)
        {
            return RenderThreadTaskFactory.StartNew(() =>
            {
                return DoEvaluateScript(frameId, script);
            }, TaskCreationOptions.AttachedToParent)
            .WithTimeout(timeout);
        }

        protected abstract object DoEvaluateScript(long frameId, string script);
    }
}

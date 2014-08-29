// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;

namespace CefSharp
{
    public abstract class CefBrowserBase : DisposableResource
    {
        public int BrowserId { get; set; }
        public TaskFactory RenderThreadTaskFactory { get; protected set; }

        public Task<object> EvaluateScript(long frameId, string script, double timeout)
        {
            return RenderThreadTaskFactory.StartNew(() =>
            {
                return DoEvaluateScript(frameId, script);
            }, TaskCreationOptions.AttachedToParent);
        }

        protected abstract object DoEvaluateScript(long frameId, string script);
    }
}

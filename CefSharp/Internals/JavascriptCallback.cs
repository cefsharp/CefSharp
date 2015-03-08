// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [DataContract]
    internal sealed class JavascriptCallback : IJavascriptCallback
    {
        private bool disposed;

        public long Id { get; set; }

        public int BrowserId { get; set; }

        public BrowserProcessServiceHost BrowserProcessServiceHost { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeInternal();
        }

        public Task<JavascriptResponse> ExecuteAsync(params object[] parms)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("JavascriptCallback is already disposed.");
            }
            return BrowserProcessServiceHost.JavascriptCallback(BrowserId, Id, parms, null);
        }

        private void DisposeInternal()
        {
            if (!disposed && BrowserProcessServiceHost != null)
            {
                BrowserProcessServiceHost.DestroyJavascriptCallback(BrowserId, Id);
                BrowserProcessServiceHost = null;
            }
            disposed = true;
        }

        ~JavascriptCallback()
        {
            DisposeInternal();
        }
    }
}

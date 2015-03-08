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
        private readonly long id;
        private readonly int browserId;
        private readonly WeakReference _browserProcess;
        private bool disposed;

        public JavascriptCallback(long id, int browserId, BrowserProcessServiceHost browserProcess)
        {
            this.id = id;
            this.browserId = browserId;
            _browserProcess = new WeakReference(browserProcess);
        }

        private BrowserProcessServiceHost BrowserProcessServiceHost
        {
            get { return (BrowserProcessServiceHost)_browserProcess.Target; }
        }

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

            var browserProcess = BrowserProcessServiceHost;
            if (browserProcess == null)
            {
                throw new ObjectDisposedException("BrowserProcessServiceHost is already disposed.");
            }

            return browserProcess.JavascriptCallback(browserId, id, parms, null);
        }

        private void DisposeInternal()
        {
            var browserProcess = BrowserProcessServiceHost;
            if (!disposed && browserProcess != null)
            {
                browserProcess.DestroyJavascriptCallback(browserId, id);
            }
            disposed = true;
        }

        ~JavascriptCallback()
        {
            DisposeInternal();
        }
    }
}

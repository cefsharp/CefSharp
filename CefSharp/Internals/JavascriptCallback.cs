// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [DataContract]
    internal sealed class JavascriptCallback : DisposableResource, IJavascriptCallback
    {
        private readonly long id;
        private readonly int browserId;
        private readonly WeakReference browserProcess;

        public JavascriptCallback(long id, int browserId, BrowserProcessServiceHost browserProcess)
        {
            this.id = id;
            this.browserId = browserId;
            this.browserProcess = new WeakReference(browserProcess);
        }

        public Task<JavascriptResponse> ExecuteAsync(params object[] parms)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("JavascriptCallback is already disposed.");
            }

            var browserProcessLocal = (BrowserProcessServiceHost)browserProcess.Target;
            if (browserProcessLocal == null)
            {
                throw new ObjectDisposedException("BrowserProcessServiceHost is already disposed.");
            }

            return browserProcessLocal.JavascriptCallback(browserId, id, parms, null);
        }

        protected override void DoDispose(bool isDisposing)
        {
            var browserProcessLocal = (BrowserProcessServiceHost)browserProcess.Target;
            if (!IsDisposed && browserProcessLocal != null)
            {
                browserProcessLocal.DestroyJavascriptCallback(browserId, id);
            }
        }
    }
}

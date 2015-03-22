// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// JavascriptCallbackProxy is a wrapper class around the WCF BrowserProcess.
    /// When the render process detects a function as param or return type it will add a 
    /// <see cref="JavascriptCallback"/> that identifies the function. On the server side
    /// WCF Transforms <see cref="JavascriptCallback"/> into <see cref="JavascriptCallbackProxy"/>
    /// using the <see cref="JavascriptCallbackSurrogate"/> class.
    /// </summary>
    [DataContract]
    internal sealed class JavascriptCallbackProxy : DisposableResource, IJavascriptCallback
    {
        private readonly long id;
        private readonly int browserId;
        private readonly WeakReference browserProcessWeakReference;

        public JavascriptCallbackProxy(long id, int browserId, WeakReference browserProcessWeakReference)
        {
            this.id = id;
            this.browserId = browserId;
            this.browserProcessWeakReference = browserProcessWeakReference;
        }

        public Task<JavascriptResponse> ExecuteAsync(params object[] parms)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("JavascriptCallbackProxy is already disposed.");
            }

            var browserProcess = (BrowserProcessServiceHost)browserProcessWeakReference.Target;
            if (browserProcess == null)
            {
                throw new ObjectDisposedException("BrowserProcessServiceHost is already disposed.");
            }

            return browserProcess.JavascriptCallback(browserId, id, parms, null);
        }

        protected override void DoDispose(bool isDisposing)
        {
            var browserProcess = (BrowserProcessServiceHost)browserProcessWeakReference.Target;
            if (!IsDisposed && browserProcess != null && browserProcess.State == CommunicationState.Opened)
            {
                browserProcess.DestroyJavascriptCallback(browserId, id);
            }
        }
    }
}

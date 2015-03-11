// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    [DataContract]
    public sealed class JavascriptCallback : DisposableResource, IJavascriptCallback
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int BrowserId { get; set; }

        public WeakReference BrowserProcess { get; set; }

        public Task<JavascriptResponse> ExecuteAsync(params object[] parms)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("JavascriptCallback is already disposed.");
            }

            var browserProcess = (BrowserProcessServiceHost)BrowserProcess.Target;
            if (browserProcess == null)
            {
                throw new ObjectDisposedException("BrowserProcessServiceHost is already disposed.");
            }

            return browserProcess.JavascriptCallback(BrowserId, Id, parms, null);
        }

        protected override void DoDispose(bool isDisposing)
        {
            var browserProcess = (BrowserProcessServiceHost)BrowserProcess.Target;
            if (!IsDisposed && browserProcess != null && browserProcess.State == CommunicationState.Opened)
            {
                browserProcess.DestroyJavascriptCallback(BrowserId, Id);
            }
        }
    }
}

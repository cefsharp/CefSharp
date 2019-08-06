// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Handler;

namespace CefSharp.Example.RequestEventHandler
{
    /// <summary>
    /// Example class that demos exposing some of the methods of <see cref="RequestHandler"/> as events.
    /// Inheriting from <see cref="RequestHandler"/> requres you only override the methods you are interested in.
    /// You can of course inherit from the interface <see cref="IRequestHandler"/> and implement all the methods
    /// yourself if that's required.
    /// Simply check out the interface method the event was named by (e.g <see cref="OnCertificateErrorEvent" /> corresponds to
    /// <see cref="IRequestHandler.OnCertificateError" />)
    /// inspired by:
    /// https://github.com/cefsharp/CefSharp/blob/fa41529853b2527eb0468a507ab6c5bd0768eb59/CefSharp.Example/RequestHandler.cs
    /// </summary>
    public class RequestEventHandler : RequestHandler
    {
        public event EventHandler<OnBeforeBrowseEventArgs> OnBeforeBrowseEvent;
        public event EventHandler<OnOpenUrlFromTabEventArgs> OnOpenUrlFromTabEvent;
        public event EventHandler<OnCertificateErrorEventArgs> OnCertificateErrorEvent;
        public event EventHandler<OnPluginCrashedEventArgs> OnPluginCrashedEvent;
        public event EventHandler<GetAuthCredentialsEventArgs> GetAuthCredentialsEvent;
        public event EventHandler<OnRenderProcessTerminatedEventArgs> OnRenderProcessTerminatedEvent;
        public event EventHandler<OnQuotaRequestEventArgs> OnQuotaRequestEvent;

        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            var args = new OnBeforeBrowseEventArgs(chromiumWebBrowser, browser, frame, request, userGesture, isRedirect);

            OnBeforeBrowseEvent?.Invoke(this, args);

            return args.CancelNavigation;
        }

        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            var args = new OnOpenUrlFromTabEventArgs(chromiumWebBrowser, browser, frame, targetUrl, targetDisposition, userGesture);

            OnOpenUrlFromTabEvent?.Invoke(this, args);

            return args.CancelNavigation;
        }

        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            var args = new OnCertificateErrorEventArgs(chromiumWebBrowser, browser, errorCode, requestUrl, sslInfo, callback);

            OnCertificateErrorEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        protected override void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
            var args = new OnPluginCrashedEventArgs(chromiumWebBrowser, browser, pluginPath);

            OnPluginCrashedEvent?.Invoke(this, args);
        }

        protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            var args = new GetAuthCredentialsEventArgs(chromiumWebBrowser, browser, originUrl, isProxy, host, port, realm, scheme, callback);

            GetAuthCredentialsEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        protected override void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            var args = new OnRenderProcessTerminatedEventArgs(chromiumWebBrowser, browser, status);

            OnRenderProcessTerminatedEvent?.Invoke(this, args);
        }

        protected override bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            var args = new OnQuotaRequestEventArgs(chromiumWebBrowser, browser, originUrl, newSize, callback);
            OnQuotaRequestEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        private static void EnsureCallbackDisposal(IRequestCallback callbackToDispose)
        {
            if (callbackToDispose != null && !callbackToDispose.IsDisposed)
            {
                callbackToDispose.Dispose();
            }
        }

        private static void EnsureCallbackDisposal(IAuthCallback callbackToDispose)
        {
            if (callbackToDispose != null && !callbackToDispose.IsDisposed)
            {
                callbackToDispose.Dispose();
            }
        }
    }
}

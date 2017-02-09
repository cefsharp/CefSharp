// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example.RequestEventHandler
{
    /// <summary>
    ///     To use this class, check <see cref="IRequestHandler" /> for more information about the event parameters.
    ///     Often you will find MANDATORY information on how to work with the parameters or which thread the call comes from.
    ///     Simply check out the interface' method the event was named by.
    ///     (e.g <see cref="RequestEventHandler.OnCertificateErrorEvent" /> corresponds to
    ///     <see cref="IRequestHandler.OnCertificateError" />)
    ///     inspired by:
    ///     https://github.com/cefsharp/CefSharp/blob/fa41529853b2527eb0468a507ab6c5bd0768eb59/CefSharp.Example/RequestHandler.cs
    /// </summary>
    public class RequestEventHandler : IRequestHandler
    {
        public event EventHandler<OnBeforeBrowseEventArgs> OnBeforeBrowseEvent;
        public event EventHandler<OnOpenUrlFromTabEventArgs> OnOpenUrlFromTabEvent;
        public event EventHandler<OnCertificateErrorEventArgs> OnCertificateErrorEvent;
        public event EventHandler<OnPluginCrashedEventArgs> OnPluginCrashedEvent;
        public event EventHandler<OnBeforeResourceLoadEventArgs> OnBeforeResourceLoadEvent;
        public event EventHandler<GetAuthCredentialsEventArgs> GetAuthCredentialsEvent;
        public event EventHandler<OnRenderProcessTerminatedEventArgs> OnRenderProcessTerminatedEvent;
        public event EventHandler<OnQuotaRequestEventArgs> OnQuotaRequestEvent;
        public event EventHandler<OnResourceRedirectEventArgs> OnResourceRedirectEvent;

        /// <summary>
        ///     SECURITY WARNING: YOU SHOULD USE THIS EVENT TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS
        ///     BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        public event EventHandler<OnProtocolExecutionEventArgs> OnProtocolExecutionEvent;
        public event EventHandler<OnRenderViewReadyEventArgs> OnRenderViewReadyEvent;
        public event EventHandler<OnResourceResponseEventArgs> OnResourceResponseEvent;
        public event EventHandler<GetResourceResponseFilterEventArgs> GetResourceResponseFilterEvent;
        public event EventHandler<OnResourceLoadCompleteEventArgs> OnResourceLoadCompleteEvent;

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            var args = new OnBeforeBrowseEventArgs(browserControl, browser, frame, request, isRedirect);
            OnBeforeBrowse(args);
            return args.CancelNavigation;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            var args = new OnOpenUrlFromTabEventArgs(browserControl, browser, frame, targetUrl, targetDisposition, userGesture);
            OnOpenUrlFromTab(args);
            return args.CancelNavigation;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            var args = new OnCertificateErrorEventArgs(browserControl, browser, errorCode, requestUrl, sslInfo, callback);
            OnCertificateError(args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            var args = new OnPluginCrashedEventArgs(browserControl, browser, pluginPath);
            OnPluginCrashed(args);
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var args = new OnBeforeResourceLoadEventArgs(browserControl, browser, frame, request, callback);
            OnBeforeResourceLoad(args);

            EnsureCallbackDisposal(callback);
            return args.ContinuationHandling;
        }

        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            var args = new GetAuthCredentialsEventArgs(browserControl, browser, frame, isProxy, host, port, realm, scheme, callback);
            OnGetAuthCredentials(args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            var args = new OnRenderProcessTerminatedEventArgs(browserControl, browser, status);
            OnRenderProcessTerminated(args);
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            var args = new OnQuotaRequestEventArgs(browserControl, browser, originUrl, newSize, callback);
            OnQuotaRequest(args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
            var args = new OnResourceRedirectEventArgs(browserControl, browser, frame, request, newUrl);
            OnResourceRedirect(args);
            if (!Equals(newUrl, args.NewUrl))
            {
                newUrl = args.NewUrl;
            }
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            var args = new OnProtocolExecutionEventArgs(browserControl, browser, url);
            OnProtocolExecution(args);
            return args.AttemptExecution;
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
            var args = new OnRenderViewReadyEventArgs(browserControl, browser);
            OnRenderViewReady(args);
        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new OnResourceResponseEventArgs(browserControl, browser, frame, request, response);
            OnResourceResponse(args);
            return args.RedirectOrRetry;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new GetResourceResponseFilterEventArgs(browserControl, browser, frame, request, response);
            OnGetResourceResponseFilter(args);
            return args.ResponseFilter;
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var args = new OnResourceLoadCompleteEventArgs(browserControl, browser, frame, request, response, status, receivedContentLength);
            OnResourceLoadComplete(args);
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

        private void OnBeforeBrowse(OnBeforeBrowseEventArgs args)
        {
            EventHandler<OnBeforeBrowseEventArgs> handler = OnBeforeBrowseEvent;
            if(handler != null)
            {
                handler(this, args);
            }
        }

        private void OnOpenUrlFromTab(OnOpenUrlFromTabEventArgs args)
        {
            EventHandler<OnOpenUrlFromTabEventArgs> handler = OnOpenUrlFromTabEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnCertificateError(OnCertificateErrorEventArgs args)
        {
            EventHandler<OnCertificateErrorEventArgs> handler = OnCertificateErrorEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPluginCrashed(OnPluginCrashedEventArgs args)
        {
            EventHandler<OnPluginCrashedEventArgs> handler = OnPluginCrashedEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnBeforeResourceLoad(OnBeforeResourceLoadEventArgs args)
        {
            EventHandler<OnBeforeResourceLoadEventArgs> handler = OnBeforeResourceLoadEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnGetAuthCredentials(GetAuthCredentialsEventArgs args)
        {
            EventHandler<GetAuthCredentialsEventArgs> handler = GetAuthCredentialsEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnRenderProcessTerminated(OnRenderProcessTerminatedEventArgs args)
        {
            EventHandler<OnRenderProcessTerminatedEventArgs> handler = OnRenderProcessTerminatedEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnQuotaRequest(OnQuotaRequestEventArgs args)
        {
            EventHandler<OnQuotaRequestEventArgs> handler = OnQuotaRequestEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnResourceRedirect(OnResourceRedirectEventArgs args)
        {
            EventHandler<OnResourceRedirectEventArgs> handler = OnResourceRedirectEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnProtocolExecution(OnProtocolExecutionEventArgs args)
        {
            EventHandler<OnProtocolExecutionEventArgs> handler = OnProtocolExecutionEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnRenderViewReady(OnRenderViewReadyEventArgs args)
        {
            EventHandler<OnRenderViewReadyEventArgs> handler = OnRenderViewReadyEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnResourceResponse(OnResourceResponseEventArgs args)
        {
            EventHandler<OnResourceResponseEventArgs> handler = OnResourceResponseEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnGetResourceResponseFilter(GetResourceResponseFilterEventArgs args)
        {
            EventHandler<GetResourceResponseFilterEventArgs> handler = GetResourceResponseFilterEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnResourceLoadComplete(OnResourceLoadCompleteEventArgs args)
        {
            EventHandler<OnResourceLoadCompleteEventArgs> handler = OnResourceLoadCompleteEvent;
            if (handler != null){
                handler(this, args);
            }
        }
    }
}

// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Security.Cryptography.X509Certificates;

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
        public event EventHandler<CanGetCookiesEventArg> CanGetCookiesEvent;
        public event EventHandler<CanSetCookieEventArg> CanSetCookieEvent;
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

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            var args = new OnBeforeBrowseEventArgs(browserControl, browser, frame, request, userGesture, isRedirect);

            OnBeforeBrowseEvent?.Invoke(this, args);

            return args.CancelNavigation;
        }

        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            var args = new OnOpenUrlFromTabEventArgs(browserControl, browser, frame, targetUrl, targetDisposition, userGesture);

            OnOpenUrlFromTabEvent?.Invoke(this, args);

            return args.CancelNavigation;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            var args = new OnCertificateErrorEventArgs(browserControl, browser, errorCode, requestUrl, sslInfo, callback);

            OnCertificateErrorEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            var args = new OnPluginCrashedEventArgs(browserControl, browser, pluginPath);

            OnPluginCrashedEvent?.Invoke(this, args);
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var args = new OnBeforeResourceLoadEventArgs(browserControl, browser, frame, request, callback);

            OnBeforeResourceLoadEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinuationHandling;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            var args = new GetAuthCredentialsEventArgs(browserControl, browser, frame, isProxy, host, port, realm, scheme, callback);

            GetAuthCredentialsEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            var args = new OnRenderProcessTerminatedEventArgs(browserControl, browser, status);

            OnRenderProcessTerminatedEvent?.Invoke(this, args);
        }

        bool IRequestHandler.CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            var args = new CanGetCookiesEventArg(browserControl, browser, frame, request);

            CanGetCookiesEvent?.Invoke(this, args);

            return args.GetCookies;
        }

        bool IRequestHandler.CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
        {
            var args = new CanSetCookieEventArg(browserControl, browser, frame, request, cookie);

            CanSetCookieEvent?.Invoke(this, args);

            return args.SetCookie;
        }

        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            var args = new OnQuotaRequestEventArgs(browserControl, browser, originUrl, newSize, callback);
            OnQuotaRequestEvent?.Invoke(this, args);

            EnsureCallbackDisposal(callback);
            return args.ContinueAsync;
        }

        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            var args = new OnResourceRedirectEventArgs(browserControl, browser, frame, request, response, newUrl);
            OnResourceRedirectEvent?.Invoke(this, args);
            if (!Equals(newUrl, args.NewUrl))
            {
                newUrl = args.NewUrl;
            }
        }

        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            var args = new OnProtocolExecutionEventArgs(browserControl, browser, url);
            OnProtocolExecutionEvent?.Invoke(this, args);
            return args.AttemptExecution;
        }

        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
            var args = new OnRenderViewReadyEventArgs(browserControl, browser);
            OnRenderViewReadyEvent?.Invoke(this, args);
        }

        bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new OnResourceResponseEventArgs(browserControl, browser, frame, request, response);
            OnResourceResponseEvent?.Invoke(this, args);
            return args.RedirectOrRetry;
        }

        IResponseFilter IRequestHandler.GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new GetResourceResponseFilterEventArgs(browserControl, browser, frame, request, response);
            GetResourceResponseFilterEvent?.Invoke(this, args);
            return args.ResponseFilter;
        }

        void IRequestHandler.OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var args = new OnResourceLoadCompleteEventArgs(browserControl, browser, frame, request, response, status, receivedContentLength);

            OnResourceLoadCompleteEvent?.Invoke(this, args);
        }

        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            //TODO: Someone please contribute an implementation of this
            throw new NotImplementedException();
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

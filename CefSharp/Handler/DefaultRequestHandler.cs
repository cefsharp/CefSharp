// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Security.Cryptography.X509Certificates;

namespace CefSharp.Handler
{
    /// <summary>
    /// Default implementation of <see cref="IRequestHandler"/>.
    /// This class provides default implementations of the methods from <see cref="IRequestHandler"/>,
    /// therefore providing a convenience base class for any custom request handler.
    /// </summary>
    public class DefaultRequestHandler : IRequestHandler
    {
        /// <summary>
        /// Called when the browser needs credentials from the user.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object that needs credentials (This will contain the URL that is being requested.)</param>
        /// <param name="isProxy">indicates whether the host is a proxy server</param>
        /// <param name="host">hostname</param>
        /// <param name="port">port number</param>
        /// <param name="realm">realm</param>
        /// <param name="scheme">scheme</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of authentication requests.</param>
        /// <returns>Return true to continue the request and call CefAuthCallback::Continue() when the authentication information is available. Return false to cancel the request. </returns>
        public virtual bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host,
            int port, string realm, string scheme, IAuthCallback callback)
        {
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// Called on the CEF IO thread to optionally filter resource response content.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame that is being redirected.</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <returns>Return an IResponseFilter to intercept this response, otherwise return null</returns>
        public virtual IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame,
            IRequest request, IResponse response)
        {
            return null;
        }

        /// <summary>
        /// Called before browser navigation.
        /// If the navigation is allowed <see cref="IWebBrowser.FrameLoadStart"/> and <see cref="IWebBrowser.FrameLoadEnd"/>
        /// will be called. If the navigation is canceled <see cref="IWebBrowser.LoadError"/> will be called with an ErrorCode
        /// value of <see cref="CefErrorCode.Aborted"/>. 
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture
        /// (e.g. clicking a link) or false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <param name="isRedirect">has the request been redirected</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        public virtual bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        /// <summary>
        /// Called before a resource request is loaded. For async processing return <see cref="CefReturnValue.ContinueAsync"/> 
        /// and execute <see cref="IRequestCallback.Continue"/> or <see cref="IRequestCallback.Cancel"/>
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>To cancel loading of the resource return <see cref="CefReturnValue.Cancel"/>
        /// or <see cref="CefReturnValue.Continue"/> to allow the resource to load normally. For async
        /// return <see cref="CefReturnValue.ContinueAsync"/></returns>
        public virtual CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IRequestCallback callback)
        {
            callback.Dispose();
            return CefReturnValue.Continue;
        }

        /// <summary>
        /// Called to handle requests for URLs with an invalid SSL certificate.
        /// Return true and call <see cref="IRequestCallback.Continue"/> either
        /// in this method or at a later time to continue or cancel the request.  
        /// If CefSettings.IgnoreCertificateErrors is set all invalid certificates
        /// will be accepted without calling this method.
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="errorCode">the error code for this invalid certificate</param>
        /// <param name="requestUrl">the url of the request for the invalid certificate</param>
        /// <param name="sslInfo">ssl certificate information</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.
        /// If empty the error cannot be recovered from and the request will be canceled automatically.</param>
        /// <returns>Return false to cancel the request immediately. Return true and use <see cref="IRequestCallback"/> to
        /// execute in an async fashion.</returns>
        public virtual bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl,
            ISslInfo sslInfo, IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// Called on the UI thread before OnBeforeBrowse in certain limited cases
        /// where navigating a new or different browser might be desirable. This
        /// includes user-initiated navigation that might open in a special way (e.g.
        /// links clicked via middle-click or ctrl + left-click) and certain types of
        /// cross-origin navigation initiated from the renderer process (e.g.
        /// navigating the top-level frame to/from a file URL).
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="targetUrl">target url</param>
        /// <param name="targetDisposition">The value indicates where the user intended to navigate the browser based
        /// on standard Chromium behaviors (e.g. current tab, new tab, etc). </param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture
        /// (e.g. clicking a link) or false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation
        /// to proceed in the source browser's top-level frame.</returns>
        public virtual bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
            WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        /// <summary>
        /// Called when a plugin has crashed
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="pluginPath">path of the plugin that crashed</param>
        public virtual void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        { }

        /// <summary>
        /// Called on the UI thread to handle requests for URLs with an unknown protocol component. 
        /// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="url">the request url</param>
        /// <returns>return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.</returns>
        public virtual bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return false;
        }

        /// <summary>
        /// Called on the CEF IO thread before sending a network request with a "Cookie"
        /// request header.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>Return true to allow cookies to be included in the network
        /// request or false to block cookies</returns>
        public virtual bool CanGetCookies(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            return true;
        }

        /// <summary>
        /// Called on the CEF IO thread when receiving a network request with a
        /// "Set-Cookie" response header value represented by cookie.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="cookie">the cookie object</param>
        /// <returns>Return true to allow the cookie to be stored or false to block the cookie.</returns>
        public virtual bool CanSetCookie(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
        {
            return true;
        }

        /// <summary>
        /// Called when JavaScript requests a specific storage quota size via the webkitStorageInfo.requestQuota function.
        /// For async processing return true and execute <see cref="IRequestCallback.Continue"/> at a later time to 
        /// grant or deny the request or <see cref="IRequestCallback.Cancel"/> to cancel.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">the origin of the page making the request</param>
        /// <param name="newSize">is the requested quota size in bytes</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>Return false to cancel the request immediately. Return true to continue the request
        /// and call <see cref="IRequestCallback.Continue"/> either in this method or at a later time to
        /// grant or deny the request.</returns>
        public virtual bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize,
            IRequestCallback callback)
        {
            callback.Dispose();
            return false;
        }

        /// <summary>
        /// Called when the render process terminates unexpectedly.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="status">indicates how the process terminated.</param>
        public virtual void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        { }

        /// <summary>
        /// Called on the CEF UI thread when the render view associated
        /// with browser is ready to receive/handle IPC messages in the render
        /// process.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        public virtual void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        { }

        /// <summary>
        /// Called on the CEF IO thread when a resource load has completed.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame that is being redirected.</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <param name="status">indicates the load completion status</param>
        /// <param name="receivedContentLength">is the number of response bytes actually read.</param>
        public virtual void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IResponse response, UrlRequestStatus status, long receivedContentLength)
        { }

        /// <summary>
        /// Called on the IO thread when a resource load is redirected. The <see cref="IRequest.Url"/>
        /// parameter will contain the old URL and other request-related information.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame that is being redirected.</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object</param>
        /// <param name="newUrl">the new URL and can be changed if desired</param>
        public virtual void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IResponse response, ref string newUrl)
        { }

        /// <summary>
        /// Called on the CEF IO thread when a resource response is received.
        /// To allow the resource to load normally return false.
        /// To redirect or retry the resource modify request (url, headers or post body) and return true.
        /// The response object cannot be modified in this callback. 
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame that is being redirected.</param>
        /// <param name="request">the request object</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <returns>
        /// To allow the resource to load normally return false.
        /// To redirect or retry the resource modify request (url, headers or post body) and return true.
        /// </returns>
        public virtual bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
            IResponse response)
        {
            return false;
        }

        /// <summary>
        /// Called when the browser needs user to select Client Certificate for authentication requests (eg. PKI authentication).
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="isProxy">indicates whether the host is a proxy server</param>
        /// <param name="host">hostname</param>
        /// <param name="port">port number</param>
        /// <param name="certificates">List of Client certificates for selection</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of client certificate selection for authentication requests.</param>
        /// <returns>Return true to continue the request and call ISelectClientCertificateCallback.Select() with the selected certificate for authentication. 
        /// Return false to use the default behavior where the browser selects the first certificate from the list. </returns>
        public virtual bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port,
            X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }
    }
}

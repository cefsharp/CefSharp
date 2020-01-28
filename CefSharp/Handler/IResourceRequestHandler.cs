// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to browser requests.
    /// The methods of this class will be called on the CEF IO thread unless otherwise indicated.
    /// </summary>
    public interface IResourceRequestHandler
    {
        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded.
        /// To optionally filter cookies for the request return a <see cref="ICookieAccessFilter"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <returns>To optionally filter cookies for the request return a ICookieAccessFilter instance otherwise return null.</returns>
        ICookieAccessFilter GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request);

        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded.
        /// To redirect or change the resource load optionally modify <paramref name="request"/>.
        /// Modification of the request URL will be treated as a redirect
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return <see cref="CefReturnValue.Continue"/> to continue the request immediately.
        /// Return <see cref="CefReturnValue.ContinueAsync"/> and call <see cref="IRequestCallback.Continue"/> or <see cref="IRequestCallback.Cancel"/> at a later time to continue or the cancel the request asynchronously.
        /// Return <see cref="CefReturnValue.Cancel"/> to cancel the request immediately.
        /// </returns>
        CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback);

        /// <summary>
        /// Called on the CEF IO thread before a resource is loaded. To specify a handler for the resource return a <see cref="IResourceHandler"/> object
        /// </summary>
        /// <param name="chromiumWebBrowser">The browser UI control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>To allow the resource to load using the default network loader return null otherwise return an instance of <see cref="IResourceHandler"/> with a valid stream</returns>
        IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request);

        /// <summary>
        /// Called on the CEF IO thread when a resource load is redirected.
        /// The <paramref name="request"/> parameter will contain the old URL and other request-related information.
        /// The <paramref name="response"/> parameter will contain the response that resulted in the
        /// redirect. The <paramref name="newUrl"/> parameter will contain the new URL and can be changed if desired. 
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <param name="newUrl">the new URL and can be changed if desired</param>
        void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl);

        /// <summary>
        /// Called on the CEF IO thread when a resource response is received.
        /// To allow the resource load to proceed without modification return false. To redirect or
        /// retry the resource load optionally modify <paramref name="request"/> and return true.
        /// Modification of the request URL will be treated as a redirect. Requests
        /// handled using the default network loader cannot be redirected in this
        /// callback. 
        ///
        /// WARNING: Redirecting using this method is deprecated. Use
        /// OnBeforeResourceLoad or GetResourceHandler to perform redirects.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <returns>
        /// To allow the resource load to proceed without modification return false. To redirect or
        /// retry the resource load optionally modify <paramref name="request"/> and return true.
        /// Modification of the request URL will be treated as a redirect.
        /// Requests handled using the default network loader cannot be redirected in this callback. 
        /// </returns>
        bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response);

        /// <summary>
        /// Called on the CEF IO thread to optionally filter resource response content.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <returns>Return an IResponseFilter to intercept this response, otherwise return null</returns>
        IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response);

        /// <summary>
        /// Called on the CEF IO thread when a resource load has completed.
        /// This method will be called for all requests, including requests that are
        /// aborted due to CEF shutdown or destruction of the associated browser. In
        /// cases where the associated browser is destroyed this callback may arrive
        /// after the <see cref="ILifeSpanHandler.OnBeforeClose"/> callback for that browser. The
        /// <see cref="IFrame.IsValid"/> method can be used to test for this situation, and care
        /// should be taken not to call <paramref name="browser"/> or <paramref name="frame"/> methods that modify state
        /// (like LoadURL, SendProcessMessage, etc.) if the frame is invalid.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="response">the response object - cannot be modified in this callback</param>
        /// <param name="status">indicates the load completion status</param>
        /// <param name="receivedContentLength">is the number of response bytes actually read.</param>
        void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength);

        /// <summary>
        /// Called on the CEF UI thread to handle requests for URLs with an unknown protocol component. 
        /// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.</returns>
        bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request);
    }
}

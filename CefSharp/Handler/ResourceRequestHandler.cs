// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Handler
{
    /// <summary>
    /// Default implementation of <see cref="IResourceRequestHandler"/>. This class provides default implementations of the methods
    /// from <see cref="IResourceRequestHandler"/>, therefore providing a convenience base class for any custom resource request
    /// handler.
    /// </summary>
    /// <seealso cref="T:CefSharp.IResourceRequestHandler"/>
    public class ResourceRequestHandler : IResourceRequestHandler
    {
        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To optionally filter cookies for the request return a
        /// <see cref="ICookieAccessFilter"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <returns>To optionally filter cookies for the request return a ICookieAccessFilter instance otherwise return null.</returns>
        ICookieAccessFilter IResourceRequestHandler.GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return GetCookieAccessFilter(chromiumWebBrowser, browser, frame, request);
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To optionally filter cookies for the request return a
        /// <see cref="ICookieAccessFilter"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <returns>To optionally filter cookies for the request return a ICookieAccessFilter instance otherwise return null.</returns>
        protected virtual ICookieAccessFilter GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return null;
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource is loaded. To specify a handler for the resource return a
        /// <see cref="IResourceHandler"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The browser UI control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource to load using the default network loader return null otherwise return an instance of
        /// <see cref="IResourceHandler"/> with a valid stream.
        /// </returns>
        IResourceHandler IResourceRequestHandler.GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return GetResourceHandler(chromiumWebBrowser, browser, frame, request);
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource is loaded. To specify a handler for the resource return a
        /// <see cref="IResourceHandler"/> object.
        /// </summary>
        /// <param name="chromiumWebBrowser">The browser UI control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource to load using the default network loader return null otherwise return an instance of
        /// <see cref="IResourceHandler"/> with a valid stream.
        /// </returns>
        protected virtual IResourceHandler GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return null;
        }

        /// <summary>Called on the CEF IO thread to optionally filter resource response content.</summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>Return an IResponseFilter to intercept this response, otherwise return null.</returns>
        IResponseFilter IResourceRequestHandler.GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
        }

        /// <summary>Called on the CEF IO thread to optionally filter resource response content.</summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>Return an IResponseFilter to intercept this response, otherwise return null.</returns>
        protected virtual IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To redirect or change the resource load optionally modify
        /// <paramref name="request"/>. Modification of the request URL will be treated as a redirect.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return <see cref="CefReturnValue.Continue"/> to continue the request immediately. Return
        /// <see cref="CefReturnValue.ContinueAsync"/> and call <see cref="IRequestCallback.Continue"/> or
        /// <see cref="IRequestCallback.Cancel"/> at a later time to continue or the cancel the request asynchronously. Return
        /// <see cref="CefReturnValue.Cancel"/> to cancel the request immediately.
        /// </returns>
        CefReturnValue IResourceRequestHandler.OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            return OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
        }

        /// <summary>
        /// Called on the CEF IO thread before a resource request is loaded. To redirect or change the resource load optionally modify
        /// <paramref name="request"/>. Modification of the request URL will be treated as a redirect.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>
        /// Return <see cref="CefReturnValue.Continue"/> to continue the request immediately. Return
        /// <see cref="CefReturnValue.ContinueAsync"/> and call <see cref="IRequestCallback.Continue"/> or
        /// <see cref="IRequestCallback.Cancel"/> at a later time to continue or the cancel the request asynchronously. Return
        /// <see cref="CefReturnValue.Cancel"/> to cancel the request immediately.
        /// </returns>
        protected virtual CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }

        /// <summary>
        /// Called on the CEF UI thread to handle requests for URLs with an unknown protocol component. SECURITY WARNING: YOU SHOULD USE
        /// THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.
        /// </returns>
        bool IResourceRequestHandler.OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return OnProtocolExecution(chromiumWebBrowser, browser, frame, request);
        }

        /// <summary>
        /// Called on the CEF UI thread to handle requests for URLs with an unknown protocol component. SECURITY WARNING: YOU SHOULD USE
        /// THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <returns>
        /// return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.
        /// </returns>
        protected virtual bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return false;
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load has completed. This method will be called for all requests, including
        /// requests that are aborted due to CEF shutdown or destruction of the associated browser. In cases where the associated browser
        /// is destroyed this callback may arrive after the <see cref="ILifeSpanHandler.OnBeforeClose"/> callback for that browser. The
        /// <see cref="IFrame.IsValid"/> method can be used to test for this situation, and care
        /// should be taken not to call <paramref name="browser"/> or <paramref name="frame"/> methods that modify state (like LoadURL,
        /// SendProcessMessage, etc.) if the frame is invalid.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="status">indicates the load completion status.</param>
        /// <param name="receivedContentLength">is the number of response bytes actually read.</param>
        void IResourceRequestHandler.OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            OnResourceLoadComplete(chromiumWebBrowser, browser, frame, request, response, status, receivedContentLength);
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load has completed. This method will be called for all requests, including
        /// requests that are aborted due to CEF shutdown or destruction of the associated browser. In cases where the associated browser
        /// is destroyed this callback may arrive after the <see cref="ILifeSpanHandler.OnBeforeClose"/> callback for that browser. The
        /// <see cref="IFrame.IsValid"/> method can be used to test for this situation, and care
        /// should be taken not to call <paramref name="browser"/> or <paramref name="frame"/> methods that modify state (like LoadURL,
        /// SendProcessMessage, etc.) if the frame is invalid.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="status">indicates the load completion status.</param>
        /// <param name="receivedContentLength">is the number of response bytes actually read.</param>
        protected virtual void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {

        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load is redirected. The <paramref name="request"/> parameter will contain the old
        /// URL and other request-related information. The <paramref name="response"/> parameter will contain the response that resulted
        /// in the redirect. The <paramref name="newUrl"/> parameter will contain the new URL and can be changed if desired.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="newUrl">[in,out] the new URL and can be changed if desired.</param>
        void IResourceRequestHandler.OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            OnResourceRedirect(chromiumWebBrowser, browser, frame, request, response, ref newUrl);
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource load is redirected. The <paramref name="request"/> parameter will contain the old
        /// URL and other request-related information. The <paramref name="response"/> parameter will contain the response that resulted
        /// in the redirect. The <paramref name="newUrl"/> parameter will contain the new URL and can be changed if desired.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object - cannot be modified in this callback.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <param name="newUrl">[in,out] the new URL and can be changed if desired.</param>
        protected virtual void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {

        }

        /// <summary>
        /// Called on the CEF IO thread when a resource response is received. To allow the resource load to proceed without modification
        /// return false. To redirect or retry the resource load optionally modify <paramref name="request"/> and return true.
        /// Modification of the request URL will be treated as a redirect. Requests handled using the default network loader cannot be
        /// redirected in this callback.
        /// 
        /// WARNING: Redirecting using this method is deprecated. Use OnBeforeResourceLoad or GetResourceHandler to perform redirects.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource load to proceed without modification return false. To redirect or retry the resource load optionally
        /// modify <paramref name="request"/> and return true. Modification of the request URL will be treated as a redirect. Requests
        /// handled using the default network loader cannot be redirected in this callback.
        /// </returns>
        bool IResourceRequestHandler.OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return OnResourceResponse(chromiumWebBrowser, browser, frame, request, response);
        }

        /// <summary>
        /// Called on the CEF IO thread when a resource response is received. To allow the resource load to proceed without modification
        /// return false. To redirect or retry the resource load optionally modify <paramref name="request"/> and return true.
        /// Modification of the request URL will be treated as a redirect. Requests handled using the default network loader cannot be
        /// redirected in this callback.
        /// 
        /// WARNING: Redirecting using this method is deprecated. Use OnBeforeResourceLoad or GetResourceHandler to perform redirects.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control.</param>
        /// <param name="browser">the browser object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="frame">the frame object - may be null if originating from ServiceWorker or CefURLRequest.</param>
        /// <param name="request">the request object.</param>
        /// <param name="response">the response object - cannot be modified in this callback.</param>
        /// <returns>
        /// To allow the resource load to proceed without modification return false. To redirect or retry the resource load optionally
        /// modify <paramref name="request"/> and return true. Modification of the request URL will be treated as a redirect. Requests
        /// handled using the default network loader cannot be redirected in this callback.
        /// </returns>
        protected virtual bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return false;
        }

        /// <summary>
        /// Called when the unamanged resource is freed.
        /// Unmanaged resources are ref counted and freed when
        /// the last reference is released, this works differently
        /// to .Net garbage collection.
        /// </summary>
        protected virtual void Dispose()
        {

        }

        void IDisposable.Dispose()
        {
            Dispose();
        }
    }
}

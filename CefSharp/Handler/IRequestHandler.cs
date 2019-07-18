// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Security.Cryptography.X509Certificates;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to browser requests.
    /// The methods of this class will be called on the thread indicated. 
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Called before browser navigation.
        /// If the navigation is allowed <see cref="IWebBrowser.FrameLoadStart"/> and <see cref="IWebBrowser.FrameLoadEnd"/>
        /// will be called. If the navigation is canceled <see cref="IWebBrowser.LoadError"/> will be called with an ErrorCode
        /// value of <see cref="CefErrorCode.Aborted"/>. 
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture
        /// (e.g. clicking a link) or false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <param name="isRedirect">has the request been redirected</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect);

        /// <summary>
        /// Called on the UI thread before OnBeforeBrowse in certain limited cases
        /// where navigating a new or different browser might be desirable. This
        /// includes user-initiated navigation that might open in a special way (e.g.
        /// links clicked via middle-click or ctrl + left-click) and certain types of
        /// cross-origin navigation initiated from the renderer process (e.g.
        /// navigating the top-level frame to/from a file URL).
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="targetUrl">target url</param>
        /// <param name="targetDisposition">The value indicates where the user intended to navigate the browser based
        /// on standard Chromium behaviors (e.g. current tab, new tab, etc). </param>
        /// <param name="userGesture">The value will be true if the browser navigated via explicit user gesture
        /// (e.g. clicking a link) or false if it navigated automatically (e.g. via the DomContentLoaded event).</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation
        /// to proceed in the source browser's top-level frame.</returns>
        bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
                              string targetUrl,
                              WindowOpenDisposition targetDisposition,
                              bool userGesture);

        /// <summary>
        /// Called on the CEF IO thread before a resource request is initiated.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">represent the source browser of the request</param>
        /// <param name="frame">represent the source frame of the request</param>
        /// <param name="request">represents the request contents and cannot be modified in this callback</param>
        /// <param name="isNavigation">will be true if the resource request is a navigation</param>
        /// <param name="isDownload">will be true if the resource request is a download</param>
        /// <param name="requestInitiator">is the origin (scheme + domain) of the page that initiated the request</param>
        /// <param name="disableDefaultHandling">to true to disable default handling of the request, in which case it will need to be handled via <see cref="IResourceRequestHandler.GetResourceHandler"/> or it will be canceled</param>
        /// <returns>To allow the resource load to proceed with default handling return null. To specify a handler for the resource return a <see cref="IResourceRequestHandler"/> object. If this callback returns null the same method will be called on the associated <see cref="IRequestContextHandler"/>, if any</returns>
        IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling);

        /// <summary>
        /// Called when the browser needs credentials from the user.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">is the origin making this authentication request</param>
        /// <param name="isProxy">indicates whether the host is a proxy server</param>
        /// <param name="host">hostname</param>
        /// <param name="port">port number</param>
        /// <param name="realm">realm</param>
        /// <param name="scheme">scheme</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of authentication requests.</param>
        /// <returns>Return true to continue the request and call <see cref="IAuthCallback.Continue(string, string)"/> when the authentication information is available. Return false to cancel the request. </returns>
        bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback);

        /// <summary>
        /// Called when JavaScript requests a specific storage quota size via the webkitStorageInfo.requestQuota function.
        /// For async processing return true and execute <see cref="IRequestCallback.Continue"/> at a later time to 
        /// grant or deny the request or <see cref="IRequestCallback.Cancel"/> to cancel.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">the origin of the page making the request</param>
        /// <param name="newSize">is the requested quota size in bytes</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>Return false to cancel the request immediately. Return true to continue the request
        /// and call <see cref="IRequestCallback.Continue"/> either in this method or at a later time to
        /// grant or deny the request.</returns>
        bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, Int64 newSize, IRequestCallback callback);

        /// <summary>
        /// Called to handle requests for URLs with an invalid SSL certificate.
        /// Return true and call <see cref="IRequestCallback.Continue"/> either
        /// in this method or at a later time to continue or cancel the request.  
        /// If CefSettings.IgnoreCertificateErrors is set all invalid certificates
        /// will be accepted without calling this method.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="errorCode">the error code for this invalid certificate</param>
        /// <param name="requestUrl">the url of the request for the invalid certificate</param>
        /// <param name="sslInfo">ssl certificate information</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.
        /// If empty the error cannot be recovered from and the request will be canceled automatically.</param>
        /// <returns>Return false to cancel the request immediately. Return true and use <see cref="IRequestCallback"/> to
        /// execute in an async fashion.</returns>
        bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback);

        /// <summary>
        /// Called when the browser needs user to select Client Certificate for authentication requests (eg. PKI authentication).
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="isProxy">indicates whether the host is a proxy server</param>
        /// <param name="host">hostname</param>
        /// <param name="port">port number</param>
        /// <param name="certificates">List of Client certificates for selection</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of client certificate selection for authentication requests.</param>
        /// <returns>Return true to continue the request and call ISelectClientCertificateCallback.Select() with the selected certificate for authentication. 
        /// Return false to use the default behavior where the browser selects the first certificate from the list. </returns>
        bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback);

        /// <summary>
        /// Called when a plugin has crashed
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="pluginPath">path of the plugin that crashed</param>
        void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath);

        /// <summary>
        /// Called on the CEF UI thread when the render view associated
        /// with browser is ready to receive/handle IPC messages in the render
        /// process.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser);

        /// <summary>
        /// Called when the render process terminates unexpectedly.
        /// </summary>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="status">indicates how the process terminated.</param>
        void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status);
    }
}

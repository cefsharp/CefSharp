// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IRequestHandler
    {
        /// <summary>
        /// Called before browser navigation.
        /// If the navigation is allowed <see cref="IWebBrowser.FrameLoadStart"/> and <see cref="IWebBrowser.FrameLoadEnd"/>
        /// will be called. If the navigation is canceled <see cref="IWebBrowser.LoadError"/> will be called with an ErrorCode
        /// value of <see cref="CefErrorCode.Aborted"/>. 
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="isRedirect">has the request been redirected</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect, IFrame frame);

        /// <summary>
        /// Called when a certificate error is thrown.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="errorCode">the error code for this invalid certificate</param>
        /// <param name="requestUrl">the url of the request for the invalid certificate</param>
        /// <returns>Return true to allow the invalid certificate and continue the request.</returns>
        bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl);

        /// <summary>
        /// Called when a plugin has crashed
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="pluginPath">path of the plugin that crashed</param>
        void OnPluginCrashed(IWebBrowser browser, string pluginPath);

        /// <summary>
        /// Called before a resource request is loaded.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="frame">The frame object</param>
        /// <remarks>
        /// The async features of this method are not currently exposed
        /// as such returning <see cref="CefReturnValue.ContinueAsync"/> should be avoided.
        /// </remarks>
        /// <returns>To cancel loading of the resource return <see cref="CefReturnValue.Cancel"/>
        /// or <see cref="CefReturnValue.Continue"/> to allow the resource to load normally.</returns>
        CefReturnValue OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IFrame frame);
        
        /// <summary>
        /// Called when the browser needs credentials from the user.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="isProxy">indicates whether the host is a proxy server</param>
        /// <param name="host">hostname</param>
        /// <param name="port">port number</param>
        /// <param name="realm">realm</param>
        /// <param name="scheme">scheme</param>
        /// <param name="username">requested username</param>
        /// <param name="password">requested password</param>
        /// <returns>Return true to continue the request and call CefAuthCallback::Continue() when the authentication information is available. Return false to cancel the request. </returns>
        bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password);

        /// <summary>
        /// Called on the browser process IO thread before a plugin is loaded.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="url">URL</param>
        /// <param name="policyUrl">policy URL</param>
        /// <param name="info">plugin information</param>
        /// <returns>Return true to block loading of the plugin.</returns>
        bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info);

        /// <summary>
        /// Called when the render process terminates unexpectedly.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="status">indicates how the process terminated.</param>
        void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status);

        /// <summary>
        /// Called when JavaScript requests a specific storage quota size via the webkitStorageInfo.requestQuota function.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">the origin of the page making the request</param>
        /// <param name="newSize">is the requested quota size in bytes</param>
        /// <returns>Return true grant the request. Return false to deny the request.</returns>
        bool OnQuotaRequest(IWebBrowser browser, string originUrl, Int64 newSize);

        /// <summary>
        /// Called on the IO thread when a resource load is redirected. The |old_url| parameter will contain . . 
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="isMainFrame">whether the request comes from main frame or not</param>
        /// <param name="oldUrl">the old URL</param>
        /// <param name="newUrl">the new URL and can be changed if desired</param>
        void OnResourceRedirect(IWebBrowser browser, bool isMainFrame, string oldUrl, ref string newUrl);

        /// <summary>
        /// Called on the UI thread to handle requests for URLs with an unknown protocol component. 
        /// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="url">the request url</param>
        /// <returns>return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.</returns>
        bool OnProtocolExecution(IWebBrowser browser, string url);
    }
}

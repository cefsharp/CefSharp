﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

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
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="isRedirect">has the request been redirected</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect);

        /// <summary>
        /// Called to handle requests for URLs with an invalid SSL certificate.
        /// Return true and call <see cref="IRequestCallback.Continue"/> either
        /// in this method or at a later time to continue or cancel the request.  
        /// If <see cref="CefSettings.IgnoreCertificateErrors"/> is set all invalid certificates
        /// will be accepted without calling this method.
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="errorCode">the error code for this invalid certificate</param>
        /// <param name="requestUrl">the url of the request for the invalid certificate</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.
        /// If empty the error cannot be recovered from and the request will be canceled automatically.</param>
        /// <returns>Return false to cancel the request immediately. Return true and use <see cref="IRequestCallback"/> to
        /// execute in an async fashion.</returns>
        bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, IRequestCallback callback);

        /// <summary>
        /// Called when a plugin has crashed
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="pluginPath">path of the plugin that crashed</param>
        void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath);

        /// <summary>
        /// Called before a resource request is loaded. For async processing return <see cref="CefReturnValue.ContinueAsync"/> 
        /// and execute <see cref="IRequestCallback.Continue"/> or <see cref="IRequestCallback.Cancel"/>
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="request">the request object - can be modified in this callback.</param>
        /// <param name="frame">The frame object</param>
        /// <param name="callback">Callback interface used for asynchronous continuation of url requests.</param>
        /// <returns>To cancel loading of the resource return <see cref="CefReturnValue.Cancel"/>
        /// or <see cref="CefReturnValue.Continue"/> to allow the resource to load normally. For async
        /// return <see cref="CefReturnValue.ContinueAsync"/></returns>
        CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback);

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
        bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback);

        /// <summary>
        /// Called on the browser process IO thread before a plugin is loaded.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="url">URL</param>
        /// <param name="policyUrl">policy URL</param>
        /// <param name="info">plugin information</param>
        /// <returns>Return true to block loading of the plugin.</returns>
        bool OnBeforePluginLoad(IWebBrowser browserControl, IBrowser browser, string url, string policyUrl, WebPluginInfo info);

        /// <summary>
        /// Called when the render process terminates unexpectedly.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="status">indicates how the process terminated.</param>
        void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status);

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
        bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, Int64 newSize, IRequestCallback callback);

        /// <summary>
        /// Called on the IO thread when a resource load is redirected. The <see cref="IRequest.Url"/>
        /// parameter will contain the old URL and other request-related information.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame that is being redirected.</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="newUrl">the new URL and can be changed if desired</param>
        void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl);

        /// <summary>
        /// Called on the UI thread to handle requests for URLs with an unknown protocol component. 
        /// SECURITY WARNING: YOU SHOULD USE THIS METHOD TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="url">the request url</param>
        /// <returns>return to true to attempt execution via the registered OS protocol handler, if any. Otherwise return false.</returns>
        bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url);

        /// <summary>
        /// Called when the page icon changes.
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="urls">list of urls where the favicons can be downloaded</param>
        void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls);
    }
}

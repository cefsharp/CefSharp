﻿using System.Net;

namespace CefSharp
{
    public enum NavigationType
    {
        LinkClicked,
        FormSubmitted,
        BackForward,
        Reload,
        FormResubmitted,
        Other
    };

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
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect);

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
        /// <param name="requestResponse">the request response object - can be modified in this callback</param>
        /// <returns>To cancel loading of the resource return true or false o allow the resource to load normally.</returns>
        bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse);
        
        // TODO: Investigate how we can support in CEF3.
        //void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers);

        /// <summary>
        /// Called when a server indicates via the 'Content-Disposition' header that a response represents a file to download.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="handler">Set instance of IDownloadHandler that will recieve the file contents</param>
        /// <returns>Return true to download the file or false to cancel the file download</returns>
        bool GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler);

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
        bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, IWebPluginInfo info);
    }
}

// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to provide handler implementations. The handler
    /// instance will not be released until all objects related to the context have
    /// been destroyed. Implement this interface to cancel loading of specific plugins
    /// </summary>
    public interface IRequestContextHandler
    {
        /// <summary>
        /// Called immediately after the request context has been initialized.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread.
        /// </summary>
        /// <param name="requestContext">the request context</param>
        void OnRequestContextInitialized(IRequestContext requestContext);

        /// <summary>
        /// Called on the CEF IO thread before a plugin instance is loaded.
        /// The default plugin policy can be set at runtime using the `--plugin-policy=[allow|detect|block]` command-line flag.
        /// </summary>
        /// <param name="mimeType">is the mime type of the plugin that will be loaded</param>
        /// <param name="url">is the content URL that the plugin will load and may be empty</param>
        /// <param name="isMainFrame">will be true if the plugin is being loaded in the main (top-level) frame</param>
        /// <param name="topOriginUrl">is the URL for the top-level frame that contains the plugin</param>
        /// <param name="pluginInfo">includes additional information about the plugin that will be loaded</param>
        /// <param name="pluginPolicy">Modify and return true to change the policy.</param>
        /// <returns>Return false to use the recommended policy. Modify and return true to change the policy.</returns>
        bool OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy);

        /// <summary>
        /// Called on the CEF IO thread before a resource request is initiated.
        /// This method will not be called if the client associated with <paramref name="browser"/> returns a non-NULL value
        /// from <see cref="IRequestHandler.GetResourceRequestHandler"/> for the same request (identified by <see cref="IRequest.Identifier"/>).
        /// </summary>
        /// <param name="browser">represent the source browser of the request, and may be null for requests originating from service workers.</param>
        /// <param name="frame">represent the source frame of the request, and may be null for requests originating from service workers.</param>
        /// <param name="request">represents the request contents and cannot be modified in this callback</param>
        /// <param name="isNavigation">will be true if the resource request is a navigation</param>
        /// <param name="isDownload">will be true if the resource request is a download</param>
        /// <param name="requestInitiator">is the origin (scheme + domain) of the page that initiated the request</param>
        /// <param name="disableDefaultHandling">Set to true to disable default handling of the request, in which case it will need to be handled via <see cref="IResourceRequestHandler.GetResourceHandler(IWebBrowser, IBrowser, IFrame, IRequest)"/> or it will be canceled</param>
        /// <returns>To allow the resource load to proceed with default handling return null. To specify a handler for the resource return a <see cref="IResourceRequestHandler"/> object.</returns>
        IResourceRequestHandler GetResourceRequestHandler(IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling);
    }
}

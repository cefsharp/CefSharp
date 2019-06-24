// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class that creates <see cref="IResourceHandler"/> instances for handling custom requests.
    /// The methods of this class will always be called on the CEF IO thread. This interface
    /// maps to the  CefRequestHandler::GetResourceHandler method. It was split out to allow for
    /// the <see cref="ResourceRequestHandlerFactory"/> implementation that provides support
    /// for the LoadHtml extension method.
    /// </summary>
    public interface IResourceRequestHandlerFactory
    {
        /// <summary>
        /// Are there any <see cref="ResourceHandler"/>'s registered?
        /// </summary>
        bool HasHandlers { get; }

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
    }
}

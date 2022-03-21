// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to provide handler implementations. The handler
    /// instance will not be released until all objects related to the context have
    /// been destroyed.
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

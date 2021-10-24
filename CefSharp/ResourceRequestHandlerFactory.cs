// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Default implementation of <see cref="IResourceRequestHandlerFactory"/> it's used
    /// internally for the LoadHtml implementation - basically a resource handler is
    /// registered for a specific Url.
    /// </summary>
    public class ResourceRequestHandlerFactory : IResourceRequestHandlerFactory
    {
        /// <summary>
        /// Resource handler thread safe dictionary
        /// </summary>
        public ConcurrentDictionary<string, ResourceRequestHandlerFactoryItem> Handlers { get; private set; }

        /// <summary>
        /// Create a new instance of DefaultResourceHandlerFactory
        /// </summary>
        /// <param name="comparer">string equality comparer</param>
        public ResourceRequestHandlerFactory(IEqualityComparer<string> comparer = null)
        {
            Handlers = new ConcurrentDictionary<string, ResourceRequestHandlerFactoryItem>(comparer ?? StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Register a handler for the specified Url
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="data">The data in byte[] format that will be used for the response</param>
        /// <param name="mimeType">mime type</param>
        /// <param name="oneTimeUse">Whether or not the handler should be used once (true) or until manually unregistered (false)</param>
        /// <returns>returns true if the Url was successfully parsed into a Uri otherwise false</returns>
        public virtual bool RegisterHandler(string url, byte[] data, string mimeType = ResourceHandler.DefaultMimeType, bool oneTimeUse = false)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                var entry = new ResourceRequestHandlerFactoryItem(data, mimeType, oneTimeUse);

                Handlers.AddOrUpdate(uri.AbsoluteUri, entry, (k, v) => entry);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Unregister a handler for the specified Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>returns true if successfully removed</returns>
        public virtual bool UnregisterHandler(string url)
        {
            ResourceRequestHandlerFactoryItem entry;
            return Handlers.TryRemove(url, out entry);
        }

        /// <summary>
        /// Are there any <see cref="ResourceHandler"/>'s registered?
        /// </summary>
        bool IResourceRequestHandlerFactory.HasHandlers
        {
            get { return Handlers.Count > 0; }
        }

        /// <inheritdoc /> 
        IResourceRequestHandler IResourceRequestHandlerFactory.GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return GetResourceRequestHandler(chromiumWebBrowser, browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
        }

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
        protected virtual IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            try
            {
                ResourceRequestHandlerFactoryItem entry;

                if (Handlers.TryGetValue(request.Url, out entry))
                {
                    if (entry.OneTimeUse)
                    {
                        Handlers.TryRemove(request.Url, out entry);
                    }

                    return new InMemoryResourceRequestHandler(entry.Data, entry.MimeType);
                }

                return null;
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}

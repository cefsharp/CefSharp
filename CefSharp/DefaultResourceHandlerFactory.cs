// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Default implementation of <see cref="IResourceHandlerFactory"/> it's used
    /// internally for the LoadHtml implementation - basically a resource handler is
    /// registered for a specific Url.
    /// </summary>
    public class DefaultResourceHandlerFactory : IResourceHandlerFactory
    {
        /// <summary>
        /// Resource handler thread safe dictionary
        /// </summary>
        public ConcurrentDictionary<string, DefaultResourceHandlerFactoryItem> Handlers { get; private set; }

        /// <summary>
        /// Create a new instance of DefaultResourceHandlerFactory
        /// </summary>
        /// <param name="comparer">string equality comparer</param>
        public DefaultResourceHandlerFactory(IEqualityComparer<string> comparer = null)
        {
            Handlers = new ConcurrentDictionary<string, DefaultResourceHandlerFactoryItem>(comparer ?? StringComparer.OrdinalIgnoreCase);
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
                var entry = new DefaultResourceHandlerFactoryItem(data, mimeType, oneTimeUse);

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
            DefaultResourceHandlerFactoryItem entry;
            return Handlers.TryRemove(url, out entry);
        }

        /// <summary>
        /// Are there any <see cref="ResourceHandler"/>'s registered?
        /// </summary>
        public bool HasHandlers
        {
            get { return Handlers.Count > 0; }
        }

        /// <summary>
        /// Called before a resource is loaded. To specify a handler for the resource return a <see cref="ResourceHandler"/> object
        /// </summary>
        /// <param name="browserControl">The browser UI control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>To allow the resource to load normally return NULL otherwise return an instance of ResourceHandler with a valid stream</returns>
        public virtual IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            try
            {
                //If we don't have anything registered, then return null
                if (!HasHandlers)
                {
                    return null;
                }

                DefaultResourceHandlerFactoryItem entry;

                if (Handlers.TryGetValue(request.Url, out entry))
                {
                    if (entry.OneTimeUse)
                    {
                        Handlers.TryRemove(request.Url, out entry);
                    }

                    return ResourceHandler.FromByteArray(entry.Data, entry.MimeType);
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

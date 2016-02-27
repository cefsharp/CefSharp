﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CefSharp
{
    public class DefaultResourceHandlerFactory : IResourceHandlerFactory
    {
        public ConcurrentDictionary<string, IResourceHandler> Handlers { get; private set; }

        public DefaultResourceHandlerFactory(IEqualityComparer<string> comparer = null)
        {
            Handlers = new ConcurrentDictionary<string, IResourceHandler>(comparer ?? StringComparer.OrdinalIgnoreCase);
        }

        public virtual bool RegisterHandler(string url, IResourceHandler handler)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                Handlers.AddOrUpdate(uri.ToString(), handler, (k, v) => handler);
                return true;
            }
            return false;
        }

        public virtual bool UnregisterHandler(string url)
        {
            IResourceHandler handler;
            return Handlers.TryRemove(url, out handler);
        }

        public bool HasHandlers
        { 
            get { return Handlers.Count > 0; }
        }

        public virtual IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            try
            {
                IResourceHandler handler;
                Handlers.TryGetValue(request.Url, out handler);

                return handler;
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}

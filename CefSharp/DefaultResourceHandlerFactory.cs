// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public class DefaultResourceHandlerFactory : IResourceHandlerFactory
    {
        public Dictionary<string, IResourceHandler> Handlers { get; private set; }

        public DefaultResourceHandlerFactory(IEqualityComparer<string> comparer = null)
        {
            Handlers = new Dictionary<string, IResourceHandler>(comparer ?? StringComparer.OrdinalIgnoreCase);
        }

        public virtual void RegisterHandler(string url, IResourceHandler handler)
        {
            Handlers[url] = handler;
        }

        public virtual void UnregisterHandler(string url)
        {
            Handlers.Remove(url);
        }

        public bool HasHandlers
        { 
            get { return Handlers.Count > 0; }
        }

        public virtual IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            IResourceHandler handler;

            return Handlers.TryGetValue(request.Url, out handler) ? handler : null;
        }
    }
}

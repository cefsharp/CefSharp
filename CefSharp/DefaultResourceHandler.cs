// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public class DefaultResourceHandler : IResourceHandler
    {
        public Dictionary<string, ResourceHandler> Handlers { get; private set; }

        public DefaultResourceHandler(IEqualityComparer<string> comparer = null)
        {
            Handlers = new Dictionary<string, ResourceHandler>(comparer ?? StringComparer.OrdinalIgnoreCase);
        }

        public virtual void RegisterHandler(string url, ResourceHandler handler)
        {
            Handlers[url] = handler;
        }

        public virtual void UnregisterHandler(string url)
        {
            Handlers.Remove(url);
        }

        public virtual ResourceHandler GetResourceHandler(IWebBrowser browser, IRequest request)
        {
            ResourceHandler handler;

            return Handlers.TryGetValue(request.Url, out handler) ? handler : null;
        }
    }
}

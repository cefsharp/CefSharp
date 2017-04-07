// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class ResourceFactoryItem
    {
        /// <summary>
        /// The handler for a specific Url
        /// </summary>
        public IResourceHandler Handler { get; private set; }

        /// <summary>
        /// Whether or not the handler should be used once (false) or until manually unregistered (true)
        /// </summary>
        public bool Persist { get; private set; }

        /// <param name="handler">The handler for a specific Url</param>
        /// <param name="persist">Whether or not the handler should be used once (false) or until manually unregistered (true)</param>
        public ResourceFactoryItem(IResourceHandler handler, bool persist)
        {
            Handler = handler;
            Persist = persist;
        }
    }
}

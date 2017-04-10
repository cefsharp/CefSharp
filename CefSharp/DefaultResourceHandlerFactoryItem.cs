// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class DefaultResourceHandlerFactoryItem
    {
        /// <summary>
        /// The handler for a specific Url
        /// </summary>
        public IResourceHandler Handler { get; private set; }

        /// <summary>
        /// Whether or not the handler should be used once (true) or until manually unregistered (false)
        /// </summary>
        public bool OneTimeUse { get; private set; }

        /// <param name="handler">The handler for a specific Url</param>
        /// <param name="oneTimeUse">Whether or not the handler should be used once (true) or until manually unregistered (false)</param>
        public DefaultResourceHandlerFactoryItem(IResourceHandler handler, bool oneTimeUse)
        {
            Handler = handler;
            OneTimeUse = oneTimeUse;
        }
    }
}

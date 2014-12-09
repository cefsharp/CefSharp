// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IResourceHandler
    {
        void RegisterHandler(string url, ResourceHandler handler);

        void UnregisterHandler(string url);

        /// <summary>
        /// Called before a resource is loaded. To specify a handler for the resource return a ResourceHandler object
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>To allow the resource to load normally return NULL otherwise return an instance of ResourceHandler with a valid stream</returns>
        ResourceHandler GetResourceHandler(IWebBrowser browser, IRequest request);
    }
}

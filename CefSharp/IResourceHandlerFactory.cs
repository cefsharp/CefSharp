// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class that creates <see cref="IResourceHandler"/> instances for handling custom requests.
    /// The methods of this class will always be called on the CEF IO thread. This interface
    /// maps to the  CefRequestHandler::GetResourceHandler method. It was split out to allow for
    /// the <see cref="DefaultResourceHandlerFactory"/> implementation that provides support
    /// for the LoadHtml extension method.
    /// </summary>
    public interface IResourceHandlerFactory
    {
        /// <summary>
        /// Are there any <see cref="ResourceHandler"/>'s registered?
        /// </summary>
        bool HasHandlers { get; }

        /// <summary>
        /// Called before a resource is loaded. To specify a handler for the resource return a <see cref="ResourceHandler"/> object
        /// </summary>
        /// <param name="browserControl">The browser UI control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">the frame object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <returns>To allow the resource to load normally return NULL otherwise return an instance of ResourceHandler with a valid stream</returns>
        IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request);
    }
}

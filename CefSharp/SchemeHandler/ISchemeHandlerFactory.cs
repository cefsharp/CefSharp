// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Class that creates <see cref="IResourceHandler"/> instances for handling scheme requests.
    /// The methods of this class will always be called on the CEF IO thread.
    /// </summary>
    public interface ISchemeHandlerFactory
    {
        /// <summary>
        /// Return a new <see cref="IResourceHandler"/> instance to handle the request or an empty
        /// reference to allow default handling of the request.
        /// </summary>
        /// <param name="browser">the browser window that originated the
        /// request or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="frame">frame that originated the request
        /// or null if the request did not originate from a browser window
        /// (for example, if the request came from CefURLRequest).</param>
        /// <param name="schemeName">the scheme name</param>
        /// <param name="request">The request. (will not contain cookie data)</param>
        /// <returns>
        /// Return a new <see cref="IResourceHandler"/> instance to handle the request or an empty
        /// reference to allow default handling of the request
        /// </returns>
        IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request);
    }
}

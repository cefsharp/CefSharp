// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.IO;

namespace CefSharp
{
    //TODO: Eval naming for this interface, not happy with this name
    public interface IResourceHandler
    {
        /// <summary>
        /// Processes request asynchronously.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="callback">The callback used to Continue or Cancel the request (async).</param>
        /// <returns>true if the request is handled, false otherwise.</returns>
        bool ProcessRequestAsync(IRequest request, ICallback callback);

        Stream GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl);
    }
}

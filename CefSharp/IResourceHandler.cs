// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    //TODO: Eval naming for this interface, not happy with this name
    public interface IResourceHandler
    {
        /// <summary>
        /// Processes request asynchronously.
        /// The implementing method should call <see cref="IResourceHandlerResponse.ProcessRequestCallback"/> when complete.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="response">The <see cref="IResourceHandlerResponse"/> object in which the handler is supposed to place the response
        /// information.</param>
        /// <returns>true if the request is handled, false otherwise.</returns>
        bool ProcessRequestAsync(IRequest request, IResourceHandlerResponse response);
    }
}

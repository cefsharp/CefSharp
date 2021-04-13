// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <inheritdoc/>
    public class UrlRequest : CefSharp.Core.UrlRequest
    {
        /// <inheritdoc/>
        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient) : base(request, urlRequestClient)
        {
        }

        /// <inheritdoc/>
        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext) : base(request, urlRequestClient, requestContext)
        {
        }

        /// <summary>
        /// Create a new URL request that is not associated with a specific browser or frame.
        /// Use <see cref="IFrame.CreateUrlRequest(IRequest, IUrlRequestClient)"/> instead if you want the
        /// request to have this association, in which case it may be handled differently.
        /// For requests originating from the browser process: It may be intercepted by the client via <see cref="IResourceRequestHandler"/>  or <see cref="ISchemeHandlerFactory"/>.
        /// POST data may only contain only a single element of type PDE_TYPE_FILE or PDE_TYPE_BYTES.
        /// Uses the Global RequestContext
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        public IUrlRequest Create(IRequest request, IUrlRequestClient urlRequestClient)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient);
        }

        /// <summary>
        /// Create a new URL request that is not associated with a specific browser or frame.
        /// Use <see cref="IFrame.CreateUrlRequest(IRequest, IUrlRequestClient)"/> instead if you want the
        /// request to have this association, in which case it may be handled differently.
        /// For requests originating from the browser process: It may be intercepted by the client via <see cref="IResourceRequestHandler"/>  or <see cref="ISchemeHandlerFactory"/>.
        /// POST data may only contain only a single element of type PDE_TYPE_FILE or PDE_TYPE_BYTES.
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        /// <param name="requestContext">request context associated with this requets.</param>
        public IUrlRequest Create(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient, requestContext);
        }
    }
}

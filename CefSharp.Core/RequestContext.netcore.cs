// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <inheritdoc/>
    public class RequestContext : CefSharp.Core.RequestContext
    {
        /// <inheritdoc/>
        public RequestContext() : base()
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext) : base(otherRequestContext)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext, CefSharp.IRequestContextHandler requestContextHandler) : base(otherRequestContext, requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContextHandler requestContextHandler) : base(requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings) : base(settings.settings)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings, CefSharp.IRequestContextHandler requestContextHandler) : base(settings.settings, requestContextHandler)
        {
        }

        /// <summary>
        /// Creates a new RequestContextBuilder which can be used to fluently set
        /// preferences
        /// </summary>
        /// <returns>Returns a new RequestContextBuilder</returns>
        public static RequestContextBuilder Configure()
        {
            var builder = new RequestContextBuilder();

            return builder;
        }
    }
}

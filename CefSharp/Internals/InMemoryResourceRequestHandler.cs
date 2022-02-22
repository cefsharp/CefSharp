// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// InMemoryResourceRequestHandler
    /// </summary>
    public sealed class InMemoryResourceRequestHandler : IResourceRequestHandler
    {
        private readonly byte[] data;
        private readonly string mimeType;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="mimeType">mime (content) type</param>
        public InMemoryResourceRequestHandler(byte[] data, string mimeType)
        {
            this.data = data;
            this.mimeType = mimeType;
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }

        /// <inheritdoc/>
        ICookieAccessFilter IResourceRequestHandler.GetCookieAccessFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return null;
        }

        /// <inheritdoc/>
        IResourceHandler IResourceRequestHandler.GetResourceHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return ResourceHandler.FromByteArray(data, mimeType);
        }

        /// <inheritdoc/>
        IResponseFilter IResourceRequestHandler.GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return null;
        }

        /// <inheritdoc/>
        CefReturnValue IResourceRequestHandler.OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }

        /// <inheritdoc/>
        bool IResourceRequestHandler.OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return false;
        }

        /// <inheritdoc/>
        void IResourceRequestHandler.OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {

        }

        /// <inheritdoc/>
        void IResourceRequestHandler.OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {

        }

        /// <inheritdoc/>
        bool IResourceRequestHandler.OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return false;
        }
    }
}

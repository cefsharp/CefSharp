// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;

namespace CefSharp.Core
{
    /// <summary>
    /// Create instances of Public Api classes, <see cref="IBrowserSettings"/>,
    /// <see cref="IWindowInfo"/> etc.
    /// </summary>
    public static class ObjectFactory
    {
        public static readonly Type BrowserSetingsType = typeof(CefSharp.Core.BrowserSettings);
        public static readonly Type RequestContextType = typeof(CefSharp.Core.RequestContext);

        /// <summary>
        /// Create a new instance of <see cref="IBrowserSettings"/>
        /// </summary>
        /// <param name="autoDispose">Dispose of browser setings after it has been used to create a browser</param>
        /// <returns>returns new instance of <see cref="IBrowserSettings"/></returns>
        public static IBrowserSettings CreateBrowserSettings(bool autoDispose)
        {
            return new CefSharp.Core.BrowserSettings(autoDispose);
        }

        /// <summary>
        /// Create a new instance of <see cref="IWindowInfo"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IWindowInfo"/></returns>
        public static IWindowInfo CreateWindowInfo()
        {
            return new CefSharp.Core.WindowInfo();
        }

        /// <summary>
        /// Create a new instance of <see cref="IPostData"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IPostData"/></returns>
        public static IPostData CreatePostData()
        {
            return new CefSharp.Core.PostData();
        }

        /// <summary>
        /// Create a new instance of <see cref="IPostDataElement"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IPostDataElement"/></returns>
        public static IPostDataElement CreatePostDataElement()
        {
            return new CefSharp.Core.PostDataElement();
        }

        /// <summary>
        /// Create a new instance of <see cref="IRequest"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IRequest"/></returns>
        public static IRequest CreateRequest()
        {
            return new CefSharp.Core.Request();
        }

        /// <summary>
        /// Create a new instance of <see cref="IUrlRequest"/>
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        /// <returns>returns new instance of <see cref="IUrlRequest"/></returns>
        public static IUrlRequest CreateUrlRequest(IRequest request, IUrlRequestClient urlRequestClient)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient);
        }

        /// <summary>
        /// Create a new instance of <see cref="IUrlRequest"/>
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="urlRequestClient">url request client</param>
        /// <param name="requestContext">request context</param>
        /// <returns>returns new instance of <see cref="IUrlRequest"/></returns>
        public static IUrlRequest CreateUrlRequest(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext)
        {
            return new CefSharp.Core.UrlRequest(request, urlRequestClient, requestContext);
        }

        /// <summary>
        /// Create a new instance of <see cref="IDragData"/>
        /// </summary>
        /// <returns>returns new instance of <see cref="IDragData"/></returns>
        public static IDragData CreateDragData()
        {
            return Core.DragData.Create();
        }

        /// <summary>
        /// Create a new <see cref="RequestContextBuilder"/> which can be used to
        /// create a new <see cref="IRequestContext"/> in a fluent flashion.
        /// Call <see cref="RequestContextBuilder.Create"/> to create the actual
        /// <see cref="IRequestContext"/> instance
        /// </summary>
        /// <returns>RequestContextBuilder</returns>
        public static RequestContextBuilder ConfigureRequestContext()
        {
            return new RequestContextBuilder();
        }
    }
}

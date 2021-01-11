// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;

namespace CefSharp.Core
{
    public static class ObjectFactory
    {
        public static Type BrowserSetingsType = typeof(CefSharp.Core.BrowserSettings);
        public static Type RequestContextType = typeof(CefSharp.Core.RequestContext);

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
    }
}

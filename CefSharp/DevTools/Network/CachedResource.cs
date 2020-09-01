// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about the cached resource.
    /// </summary>
    public class CachedResource
    {
        /// <summary>
        /// Resource URL. This is the url of the original network request.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Type of this resource.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Cached response data.
        /// </summary>
        public Response Response
        {
            get;
            set;
        }

        /// <summary>
        /// Cached response body size.
        /// </summary>
        public long BodySize
        {
            get;
            set;
        }
    }
}
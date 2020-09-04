// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// GetCookiesResponse
    /// </summary>
    public class GetCookiesResponse
    {
        /// <summary>
        /// Array of cookie objects.
        /// </summary>
        public System.Collections.Generic.IList<Cookie> cookies
        {
            get;
            set;
        }
    }
}
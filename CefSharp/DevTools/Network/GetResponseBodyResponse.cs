// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// GetResponseBodyResponse
    /// </summary>
    public class GetResponseBodyResponse
    {
        /// <summary>
        /// Response body.
        /// </summary>
        public string body
        {
            get;
            set;
        }

        /// <summary>
        /// True, if content was sent as base64.
        /// </summary>
        public bool base64Encoded
        {
            get;
            set;
        }
    }
}
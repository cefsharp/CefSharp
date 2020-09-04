// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// GetRequestPostDataResponse
    /// </summary>
    public class GetRequestPostDataResponse
    {
        /// <summary>
        /// Request body string, omitting files from multipart requests
        /// </summary>
        public string postData
        {
            get;
            set;
        }
    }
}
// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Method Response
    /// </summary>
    public class DevToolsMethodResponse : DevToolsDomainResponseBase
    {
        /// <summary>
        /// MessageId
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Method Response as Json string
        /// </summary>
        public string ResponseAsJsonString { get; set; }
    }
}

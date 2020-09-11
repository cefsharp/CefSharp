// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Fetch
{
    /// <summary>
    /// Stages of the request to handle. Request will intercept before the request is
    /// sent. Response will intercept after the response is received (but before response
    /// body is received.
    /// </summary>
    public enum RequestStage
    {
        /// <summary>
        /// Request
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Request"))]
        Request,
        /// <summary>
        /// Response
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Response"))]
        Response
    }
}
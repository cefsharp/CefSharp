// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Stages of the interception to begin intercepting. Request will intercept before the request is
    /// sent. Response will intercept after the response is received.
    /// </summary>
    public enum InterceptionStage
    {
        /// <summary>
        /// Request
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Request"))]
        Request,
        /// <summary>
        /// HeadersReceived
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("HeadersReceived"))]
        HeadersReceived
    }
}
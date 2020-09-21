// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// The action to take when a certificate error occurs. continue will continue processing the
    /// request and cancel will cancel the request.
    /// </summary>
    public enum CertificateErrorAction
    {
        /// <summary>
        /// continue
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("continue"))]
        Continue,
        /// <summary>
        /// cancel
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("cancel"))]
        Cancel
    }
}
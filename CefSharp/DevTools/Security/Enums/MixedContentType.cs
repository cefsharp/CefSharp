// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// A description of mixed content (HTTP resources on HTTPS pages), as defined by
    /// https://www.w3.org/TR/mixed-content/#categories
    /// </summary>
    public enum MixedContentType
    {
        /// <summary>
        /// blockable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("blockable"))]
        Blockable,
        /// <summary>
        /// optionally-blockable
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("optionally-blockable"))]
        OptionallyBlockable,
        /// <summary>
        /// none
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("none"))]
        None
    }
}
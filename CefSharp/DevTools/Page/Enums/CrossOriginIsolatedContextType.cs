// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Indicates whether the frame is cross-origin isolated and why it is the case.
    /// </summary>
    public enum CrossOriginIsolatedContextType
    {
        /// <summary>
        /// Isolated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("Isolated"))]
        Isolated,
        /// <summary>
        /// NotIsolated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("NotIsolated"))]
        NotIsolated,
        /// <summary>
        /// NotIsolatedFeatureDisabled
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("NotIsolatedFeatureDisabled"))]
        NotIsolatedFeatureDisabled
    }
}
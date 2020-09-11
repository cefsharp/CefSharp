// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.ServiceWorker
{
    /// <summary>
    /// ServiceWorkerVersionStatus
    /// </summary>
    public enum ServiceWorkerVersionStatus
    {
        /// <summary>
        /// new
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("new"))]
        New,
        /// <summary>
        /// installing
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("installing"))]
        Installing,
        /// <summary>
        /// installed
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("installed"))]
        Installed,
        /// <summary>
        /// activating
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("activating"))]
        Activating,
        /// <summary>
        /// activated
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("activated"))]
        Activated,
        /// <summary>
        /// redundant
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute(Value = ("redundant"))]
        Redundant
    }
}
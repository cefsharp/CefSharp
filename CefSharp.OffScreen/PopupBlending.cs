// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Represents the popup blending in the background bitmap.
    /// </summary>
    public enum PopupBlending
    {
        /// <summary>
        /// The background and popup should be blended.
        /// </summary>
        Blend,
        /// <summary>
        /// The background and popup should be kept separated.
        /// </summary>
        Separate
    }
}

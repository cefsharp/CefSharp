// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Represents the popup blending in the main bitmap.
    /// </summary>
    public enum PopupBlending
    {
        /// <summary>
        /// The main bitmap and popup bitmap should be blended.
        /// </summary>
        Blend = 0,
        /// <summary>
        /// Retrieve the main bitmap
        /// </summary>
        Main,
        /// <summary>
        /// Retrieve the popup bitmap
        /// </summary>
        Popup
    }
}

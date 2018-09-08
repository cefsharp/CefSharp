// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    /// <summary>
    /// Describes how to interpret the alpha component of a pixel.	
    /// </summary>
    public enum AlphaType
    {
        /// <summary>
        /// No transparency. The alpha component is ignored.
        /// </summary>
        Opaque,

        /// <summary>
        /// Transparency with pre-multiplied alpha component.
        /// </summary>
        PreMultiplied,

        /// <summary>
        /// Transparency with post-multiplied alpha component.
        /// </summary>
        PostMultiplied
    }
}

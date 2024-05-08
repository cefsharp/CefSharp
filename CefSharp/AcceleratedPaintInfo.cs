// Copyright © 2024 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// Class representing accelerated paint info.
    /// </summary>
    public sealed class AcceleratedPaintInfo
    {
        /// <summary>
        /// Handle for the shared texture. The shared texture is instantiated
        /// without a keyed mutex.
        /// </summary>
        public IntPtr SharedTextureHandle { get; }

        /// <summary>
        /// The pixel format of the texture.
        /// </summary>
        public ColorType Format { get; }

        /// <summary>
        /// AcceleratedPaintInfo
        /// </summary>
        /// <param name="sharedTextureHandle">Handle to the shared texture resource</param>
        /// <param name="format">The pixel format of the shared texture resource</param>
        public AcceleratedPaintInfo(IntPtr sharedTextureHandle, ColorType format)
        {
            SharedTextureHandle = sharedTextureHandle;
            Format = format;
        }
    }
}

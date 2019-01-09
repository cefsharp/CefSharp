// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Enums
{
    /// <summary>
    /// "Verb" of a drag-and-drop operation as negotiated between the source and destination.
    /// </summary>
    [Flags]
    public enum DragOperationsMask : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Copy
        /// </summary>
        Copy = 1,
        /// <summary>
        /// Link
        /// </summary>
        Link = 2,
        /// <summary>
        /// Generic
        /// </summary>
        Generic = 4,
        /// <summary>
        /// Private
        /// </summary>
        Private = 8,
        /// <summary>
        /// Move
        /// </summary>
        Move = 16,
        /// <summary>
        /// Delete
        /// </summary>
        Delete = 32,
        /// <summary>
        /// Every drag operation.
        /// </summary>
        Every = uint.MaxValue
    }
}

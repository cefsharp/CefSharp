// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Supported context menu media types.
    /// </summary>
    public enum ContextMenuMediaType
    {
        /// <summary>
        /// No special node is in context.
        /// </summary>
        None,
        /// <summary>
        /// An image node is selected.
        /// </summary>
        Image,
        /// <summary>
        /// A video node is selected.
        /// </summary>
        Video,
        /// <summary>
        /// An audio node is selected.
        /// </summary>
        Audio,
        /// <summary>
        /// A file node is selected.
        /// </summary>
        File,
        /// <summary>
        /// A plugin node is selected.
        /// </summary>
        Plugin,
    }
}

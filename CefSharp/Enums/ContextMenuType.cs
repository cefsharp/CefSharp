// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// ContextMenuType
    /// </summary>
    [Flags]
    public enum ContextMenuType
    {
        /// <summary>
        /// No node is selected.
        /// </summary>
        None = 0,
        /// <summary>
        /// The top page is selected.
        /// </summary>
        Page = 1 << 0,
        /// <summary>
        /// A subframe page is selected.
        /// </summary>
        Frame = 1 << 1,
        /// <summary>
        /// A link is selected.
        /// </summary>
        Link = 1 << 2,
        /// <summary>
        /// A media node is selected.
        /// </summary>
        Media = 1 << 3,
        /// <summary>
        /// There is a textual or mixed selection that is selected.
        /// </summary>
        Selection = 1 << 4,
        /// <summary>
        /// An editable element is selected.
        /// </summary>
        Editable = 1 << 5,
    }
}

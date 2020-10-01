// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Supported context menu edit state bit flags.
    /// </summary>
    [Flags]
    public enum ContextMenuEditState
    {
        /// <summary>
        /// A binary constant representing the none flag.
        /// </summary>
        None = 0,
        /// <summary>
        /// A binary constant representing the can undo flag.
        /// </summary>
        CanUndo = 1 << 0,
        /// <summary>
        /// A binary constant representing the can redo flag.
        /// </summary>
        CanRedo = 1 << 1,
        /// <summary>
        /// A binary constant representing the can cut flag.
        /// </summary>
        CanCut = 1 << 2,
        /// <summary>
        /// A binary constant representing the can copy flag.
        /// </summary>
        CanCopy = 1 << 3,
        /// <summary>
        /// A binary constant representing the can paste flag.
        /// </summary>
        CanPaste = 1 << 4,
        /// <summary>
        /// A binary constant representing the can delete flag.
        /// </summary>
        CanDelete = 1 << 5,
        /// <summary>
        /// A binary constant representing the can select all flag.
        /// </summary>
        CanSelectAll = 1 << 6,
        /// <summary>
        /// A binary constant representing the can translate flag.
        /// </summary>
        CanTranslate = 1 << 7,
    }
}

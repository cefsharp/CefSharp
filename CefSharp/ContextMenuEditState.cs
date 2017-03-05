// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        None = 0,
        CanUndo = 1 << 0,
        CanRedo = 1 << 1,
        CanCut = 1 << 2,
        CanCopy = 1 << 3,
        CanPaste = 1 << 4,
        CanDelete = 1 << 5,
        CanSelectAll = 1 << 6,
        CanTranslate = 1 << 7,
    }
}

// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.Enums;

namespace CefSharp.Wpf.Internals
{
    // Workaround for upstream issue
    // It's possible resources Ids might change between versions, so this might need to be updated
    // for different builds, will need to test after upgrading
    // See #4021
    public static class EmbeddedCursor
    {
        private static Dictionary<CursorType, int> EmbeddedCursors = new Dictionary<CursorType, int>
        {
            { CursorType.Alias, 35650 },
            { CursorType.Cell, 35651 },
            { CursorType.ColumnResize, 35652 },
            { CursorType.Copy, 35653 },
            { CursorType.Grab, 35654 },
            { CursorType.Grabbing, 35655 },
            { CursorType.EastPanning, 35656 },
            { CursorType.MiddlePanning, 35657 },
            { CursorType.MiddlePanningHorizontal, 35658 },
            { CursorType.MiddlePanningVertical, 35659 },
            { CursorType.NorthPanning, 35660 },
            { CursorType.NortheastPanning, 35661 },
            { CursorType.NorthwestPanning, 35662 },
            { CursorType.SouthPanning, 35663 },
            { CursorType.SoutheastPanning, 35664 },
            { CursorType.SouthwestPanning, 35665 },
            { CursorType.WestPanning, 35666 },
            { CursorType.RowResize, 35667 },
            { CursorType.VerticalText, 35668 },
            { CursorType.ZoomIn, 35669 },
            { CursorType.ZoomOut, 35670 }
        };

        public static bool TryLoadCursor(CursorType cursorType, out IntPtr cursor)
        {
            cursor = IntPtr.Zero;

            try
            {
                if (EmbeddedCursors.TryGetValue(cursorType, out int key))
                {
                    cursor = NativeMethodWrapper.LoadCursorFromLibCef(key);

                    return cursor != IntPtr.Zero;
                }
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}

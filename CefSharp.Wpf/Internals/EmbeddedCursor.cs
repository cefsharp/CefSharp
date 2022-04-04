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
        //const int IDC_ALIAS = 35610;
        //const int IDC_CELL = 35611;
        //const int IDC_COLRESIZE = 35612;
        //const int IDC_COPYCUR = 35613;
        //const int IDC_HAND_GRAB = 35614;
        //const int IDC_HAND_GRABBING = 35615;
        //const int IDC_PAN_EAST = 35616;
        //const int IDC_PAN_MIDDLE = 35617;
        //const int IDC_PAN_MIDDLE_HORIZONTAL = 35618;
        //const int IDC_PAN_MIDDLE_VERTICAL = 35619;
        //const int IDC_PAN_NORTH = 35620;
        //const int IDC_PAN_NORTH_EAST = 35621;
        //const int IDC_PAN_NORTH_WEST = 35622;
        //const int IDC_PAN_SOUTH = 35623;
        //const int IDC_PAN_SOUTH_EAST = 35624;
        //const int IDC_PAN_SOUTH_WEST = 35625;
        //const int IDC_PAN_WEST = 35626;
        //const int IDC_ROWRESIZE = 35627;
        //const int IDC_VERTICALTEXT = 35628;
        //const int IDC_ZOOMIN = 35629;
        //const int IDC_ZOOMOUT = 35630;

        private static Dictionary<CursorType, int> EmbeddedCursors = new Dictionary<CursorType, int>
        {
            { CursorType.Alias, 35610 },
            { CursorType.Cell, 35611 },
            { CursorType.ColumnResize, 35612 },
            { CursorType.Copy, 35613 },
            { CursorType.Grab, 35614 },
            { CursorType.Grabbing, 35615 },
            { CursorType.EastPanning, 35616 },
            { CursorType.MiddlePanning, 35617 },
            { CursorType.MiddlePanningHorizontal, 35618 },
            { CursorType.MiddlePanningVertical, 35619 },
            { CursorType.NorthPanning, 35620 },
            { CursorType.NortheastPanning, 35621 },
            { CursorType.NorthwestPanning, 35622 },
            { CursorType.SouthPanning, 35623 },
            { CursorType.SoutheastPanning, 35624 },
            { CursorType.SouthwestPanning, 35625 },
            { CursorType.WestPanning, 35626 },
            { CursorType.RowResize, 35627 },
            { CursorType.VerticalText, 35628 },
            { CursorType.ZoomIn, 35629 },
            { CursorType.ZoomOut, 35630 }
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

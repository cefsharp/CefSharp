// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using CefSharp.Enums;
using Microsoft.Win32.SafeHandles;

namespace CefSharp.Wpf.Internals
{

    internal static class DragCursorProvider 
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, ushort lpCursorName);
        
        private static readonly Dictionary<DragDropEffects, Cursor> DragCursors;

        static DragCursorProvider() 
        {
            var library = LoadLibrary("ole32.dll");
            DragCursors = new Dictionary<DragDropEffects, Cursor>()
            {
                { DragDropEffects.None, GetCursorFromLib(library, 1) },
                { DragDropEffects.Move, GetCursorFromLib(library, 2) },
                { DragDropEffects.Copy, GetCursorFromLib(library, 3) },
                { DragDropEffects.Link, GetCursorFromLib(library, 4) }
                // TODO: support black cursors
            };
        }

        private static Cursor GetCursorFromLib(IntPtr library, ushort cursorIndex) 
        {
            var cursorHandle = LoadCursor(library, cursorIndex);
            return CursorInteropHelper.Create(new SafeFileHandle(cursorHandle, false));
        }

        /// <summary>
        /// Get the Windows cursor for the drag effect specified.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>The drop cursor based on the specified drag operation effect</returns>
        public static Cursor GetCursor(DragOperationsMask operation) 
        {
            var effects = operation.GetDragEffects();

            Cursor cursor;
            if (DragCursors.TryGetValue(effects, out cursor)) 
            {
                return cursor;
            }
            return Cursors.Arrow;
        }
    }
}

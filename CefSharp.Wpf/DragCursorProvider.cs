using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using CefSharp.Enums;
using Microsoft.Win32.SafeHandles;

namespace CefSharp.Wpf 
{

    internal static class DragCursorProvider 
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, ushort lpCursorName);
        
        private static readonly Dictionary<DragOperationsMask, Cursor> DragCursors;

        static DragCursorProvider() 
        {
            var library = LoadLibrary("ole32.dll");
            DragCursors = new Dictionary<DragOperationsMask, Cursor>()
            {
                { DragOperationsMask.None, GetCursorFromLib(library, 1) },
                { DragOperationsMask.Move, GetCursorFromLib(library, 2) },
                { DragOperationsMask.Copy, GetCursorFromLib(library, 3) },
                { DragOperationsMask.Link, GetCursorFromLib(library, 4) }
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
            Cursor cursor;
            if (DragCursors.TryGetValue(operation, out cursor)) 
            {
                return cursor;
            }
            return Cursors.Arrow;
        }
    }
}

// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CefSharp.WinForms.Internals
{
    public static class User32
    {
        [DllImport("User32.dll")]
        public extern static IntPtr GetFocus();

        [DllImport("user32.dll")]
        public extern static bool IsChild(IntPtr parent, IntPtr child);


        /// <summary>
        /// Gets a window using relative positions
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public extern static IntPtr GetWindow(IntPtr hWnd, GetWindowPosition uCmd);

        /// <summary>
        /// Possible flags for GetWindow
        /// </summary>
        public enum GetWindowPosition : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// Get recursively all child windows of the specified hWnd
        /// </summary>
        public static void GetSubWindows(IntPtr hWnd, List<IntPtr> subWindows)
        {
            IntPtr childWindow = GetWindow(hWnd, GetWindowPosition.GW_CHILD);

            while (childWindow != IntPtr.Zero)
            {
                subWindows.Add(childWindow);
                GetSubWindows(childWindow, subWindows);

                childWindow = GetWindow(childWindow, GetWindowPosition.GW_HWNDNEXT);
            }
        }
    }
}
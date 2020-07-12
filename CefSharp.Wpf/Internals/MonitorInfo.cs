// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace CefSharp.Wpf.Internals
{
    /// <summary>
    /// MonitorInfo is a wrapper class around MonitorFromWindow and GetMonitorInfo
    /// https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-monitorfromwindow
    /// https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getmonitorinfoa
    /// </summary>
    internal static class MonitorInfo
    {
        private const int MONITOR_DEFAULTTONULL = 0;
        private const int MONITOR_DEFAULTTOPRIMARY = 1;
        private const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        /// <summary>
        /// Gets monitor information for the provided window handle
        /// </summary>
        /// <param name="windowHandle">window handle</param>
        /// <param name="monitorInfo">monitor info</param>
        internal static void GetMonitorInfoForWindowHandle(IntPtr windowHandle, ref MonitorInfoEx monitorInfo)
        {
            var hMonitor = MonitorFromWindow(windowHandle, MONITOR_DEFAULTTONEAREST);
            GetMonitorInfo(hMonitor, ref monitorInfo);
        }
    }
}

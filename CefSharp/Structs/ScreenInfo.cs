// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    /// <summary>
    /// Class representing the virtual screen information for use when window
    /// rendering is disabled.
    /// </summary>
    /// <remarks>
    /// See also <a href="https://cs.chromium.org/chromium/src/content/public/common/screen_info.h?q=content::ScreenInfo&amp;sq=package:chromium&amp;g=0&amp;l=19">Chrome Source</a>
    /// </remarks>
    public struct ScreenInfo
    {
        /// <summary>
        /// Device scale factor. Specifies the ratio between physical and logical pixels.
        /// </summary>
        public float DeviceScaleFactor { get; set; }
        /// <summary>
        /// The screen depth in bits per pixel.
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// The bits per color component. This assumes that the colors are balanced equally.
        /// </summary>
        public int DepthPerComponent { get; set; }
        /// <summary>
        /// This can be true for black and white printers.
        /// </summary>
        public bool IsMonochrome { get; set; }
        /// <summary>
        /// This is set from the rcMonitor member of MONITORINFOEX, to whit:
        /// "A RECT structure that specifies the display monitor rectangle,
        /// expressed in virtual-screen coordinates. Note that if the monitor
        /// is not the primary display monitor, some of the rectangle's
        /// coordinates may be negative values."
        /// The Rect and AvailableRect properties are used to determine the
        /// available surface for rendering popup views.
        /// </summary>
        public Rect? Rect { get; set; }
        /// <summary>
        /// This is set from the rcWork member of MONITORINFOEX, to whit:
        /// "A RECT structure that specifies the work area rectangle of the
        /// display monitor that can be used by applications, expressed in
        /// virtual-screen coordinates. Windows uses this rectangle to
        /// maximize an application on the monitor. The rest of the area in
        /// rcMonitor contains system windows such as the task bar and side
        /// bars. Note that if the monitor is not the primary display monitor,
        /// some of the rectangle's coordinates may be negative values".
        ///
        /// The Rect and AvailableRect properties are used to determine the
        /// available surface for rendering popup views.
        /// </summary>
        public Rect? AvailableRect { get; set; }
    }
}

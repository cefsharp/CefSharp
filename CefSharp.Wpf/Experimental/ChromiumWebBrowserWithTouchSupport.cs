// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;
using CefSharp.Enums;
using CefSharp.Structs;
using CefSharp.Wpf.Internals;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// An Experimental ChromiumWebBrowser implementation that includes support for Stylus
    /// using the default WPF touch implementation. There are known performance problems with
    /// this default implementation, workarounds such as https://github.com/jaytwo/WmTouchDevice
    /// may need to be considered. .Net 4.7 supports the newer WM_Pointer implementation which
    /// should resolve the issue see https://github.com/dotnet/docs/blob/master/docs/framework/migration-guide/mitigation-pointer-based-touch-and-stylus-support.md
    /// Original PR https://github.com/cefsharp/CefSharp/pull/2745
    /// Original Author https://github.com/GSonofNun
    /// Touch support was merged into ChromiumWebBrowser, only Style support still exists in this class
    /// </summary>
    public class ChromiumWebBrowserWithTouchSupport : ChromiumWebBrowser
    {
        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.StylusDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.StylusDownEventArgs" /> that contains the event data.</param>
        protected override void OnStylusDown(StylusDownEventArgs e)
        {
            //Both touch and stylus will raise stylus event.
            //We use the OnTouchXXX methods which contain more intermediate points to handle the user's touch so that we can track the user's fingers faster.
            //Use e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus to ensure only stylus pen.
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                Focus();
                // Capture stylus so stylus events are still pushed to CEF even if the stylus leaves the control before a StylusUp.
                // This behavior is similar to how other browsers handle stylus input.
                CaptureStylus();
                OnStylus(e, TouchEventType.Pressed);
            }
            base.OnStylusDown(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.StylusMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.StylusDownEventArgs" /> that contains the event data.</param>
        protected override void OnStylusMove(StylusEventArgs e)
        {
            //Both touch and stylus will raise stylus event.
            //We use the OnTouchXXX methods which contain more intermediate points to handle the user's touch so that we can track the user's fingers faster.
            //Use e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus to ensure only stylus pen.
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                OnStylus(e, TouchEventType.Moved);
            }
            base.OnStylusMove(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.StylusUp" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.StylusDownEventArgs" /> that contains the event data.</param>
        protected override void OnStylusUp(StylusEventArgs e)
        {
            //Both touch and stylus will raise stylus event.
            //We use the OnTouchXXX methods which contain more intermediate points to handle the user's touch so that we can track the user's fingers faster.
            //Use e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus to ensure only stylus pen.
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                ReleaseStylusCapture();
                OnStylus(e, TouchEventType.Released);
            }
            base.OnStylusUp(e);
        }

        /// <summary>
        /// Handles a <see cref="E:Stylus" /> event.
        /// </summary>
        /// <param name="e">The <see cref="StylusEventArgs"/> instance containing the event data.</param>
        /// <param name="touchEventType">The <see cref="TouchEventType"/> event type</param>
        private void OnStylus(StylusEventArgs e, TouchEventType touchEventType)
        {
            var browser = GetBrowser();

            if (!e.Handled && browser != null)
            {
                var modifiers = WpfExtensions.GetModifierKeys();
                var pointerType = e.StylusDevice.Inverted ? PointerType.Eraser : PointerType.Pen;
                //Send all points to host
                foreach (var stylusPoint in e.GetStylusPoints(this))
                {
                    var touchEvent = new TouchEvent()
                    {
                        Id = e.StylusDevice.Id,
                        X = (float)stylusPoint.X,
                        Y = (float)stylusPoint.Y,
                        PointerType = pointerType,
                        Pressure = stylusPoint.PressureFactor,
                        Type = touchEventType,
                        Modifiers = modifiers,
                    };

                    browser.GetHost().SendTouchEvent(touchEvent);
                }
                e.Handled = true;
            }
        }
    }
}

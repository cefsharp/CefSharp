// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Input;
using CefSharp.Internals;
using CefSharp.Wpf.Internals;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// ChromiumWebBrowserWithMouseCapture - Captures the mouse upon left button down to provide
    /// for dragging the scrollbars/dragging the page down outside of the browser bounds.
    /// See https://github.com/cefsharp/CefSharp/issues/2258 for more details.
    /// If using on a Device that has a touch screen you will need to disable WPF StylusAndTouchSupport
    /// until https://github.com/dotnet/wpf/issues/1323 is resolved.
    /// </summary>
    [Obsolete("Only here for testing purposes, will be removed soon as https://github.com/cefsharp/CefSharp/issues/2258 is resolved")]
    public class ChromiumWebBrowserWithMouseCapture : ChromiumWebBrowser
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            //We should only need to capture the left button exiting the browser
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                //Capture/Release Mouse to allow for scrolling outside bounds of browser control (#2870, #2060).
                //Known issue when capturing and the device has a touch screen, to workaround this issue
                //disable WPF StylusAndTouchSupport see for details https://github.com/dotnet/wpf/issues/1323#issuecomment-513870984
                CaptureMouse();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
            {
                //Release the mouse capture that we grabbed on mouse down.
                //We won't get here if e.g. the right mouse button is pressed and released
                //while the left is still held, but in that case the left mouse capture seems
                //to be released implicitly (even without the left mouse SendMouseClickEvent in leave below)
                //Use ReleaseMouseCapture over Mouse.Capture(null); as it has additional Mouse.Captured == this check
                ReleaseMouseCapture();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (!e.Handled && !IsDisposed)
            {
                var modifiers = e.GetModifiers();
                var point = e.GetPosition(this);

                var host = this.GetBrowserHost();
                if (host != null && !host.IsDisposed)
                {
                    host.SendMouseMoveEvent((int)point.X, (int)point.Y, true, modifiers);
                }

                ((IWebBrowserInternal)this).SetTooltipText(null);
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);

            if (!e.Handled && !IsDisposed)
            {
                var host = this.GetBrowserHost();
                if (host != null && !host.IsDisposed)
                {
                    host.SendCaptureLostEvent();
                }
            }
        }
    }
}

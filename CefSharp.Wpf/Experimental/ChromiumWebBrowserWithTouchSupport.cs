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
    /// An Experimental ChromiumWebBrowser implementation that includes support for Touch/Stylus
    /// using the default WPF touch implementation. There are known performance problems with
    /// this default implementation, workarounds such as https://github.com/jaytwo/WmTouchDevice
    /// may need to be considered. .Net 4.7 supports the newer WM_Pointer implementation which
    /// should resolve the issue see https://github.com/dotnet/docs/blob/master/docs/framework/migration-guide/mitigation-pointer-based-touch-and-stylus-support.md
    /// Original PR https://github.com/cefsharp/CefSharp/pull/2745
    /// Original Author https://github.com/GSonofNun
    /// </summary>
    public class ChromiumWebBrowserWithTouchSupport : ChromiumWebBrowser
    {
        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Mouse, touch, and stylus will raise mouse event.
            //For mouse events from an actual mouse, e.StylusDevice will be null.
            //For mouse events from touch and stylus, e.StylusDevice will not be null.
            //We only handle event from mouse here.
            //If not, touch will cause duplicate events (mousemove and touchmove) and so does stylus.
            //Use e.StylusDevice == null to ensure only mouse.
            if (e.StylusDevice == null)
            {
                base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            //Mouse, touch, and stylus will raise mouse event.
            //For mouse events from an actual mouse, e.StylusDevice will be null.
            //For mouse events from touch and stylus, e.StylusDevice will not be null.
            //We only handle event from mouse here.
            //If not, touch will cause duplicate events (mouseup and touchup) and so does stylus.
            //Use e.StylusDevice == null to ensure only mouse.
            if (e.StylusDevice == null)
            {
                base.OnMouseUp(e);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an
        /// element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data.
        /// This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //Mouse, touch, and stylus will raise mouse event.
            //For mouse events from an actual mouse, e.StylusDevice will be null.
            //For mouse events from touch and stylus, e.StylusDevice will not be null.
            //We only handle event from mouse here.
            //If not, touch will cause duplicate events (mouseup and touchup) and so does stylus.
            //Use e.StylusDevice == null to ensure only mouse.
            if (e.StylusDevice == null)
            {
                base.OnMouseDown(e);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            //Mouse, touch, and stylus will raise mouse event.
            //For mouse events from an actual mouse, e.StylusDevice will be null.
            //For mouse events from touch and stylus, e.StylusDevice will not be null.
            //We only handle event from mouse here.
            //OnMouseLeave event from touch or stylus needn't to be handled.
            //Use e.StylusDevice == null to ensure only mouse.
            if (e.StylusDevice == null)
            {
                base.OnMouseLeave(e);
            }
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.TouchDown" /> routed event that occurs when a touch presses inside this element.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.TouchEventArgs" /> that contains the event data.</param>
        protected override void OnTouchDown(TouchEventArgs e)
        {
            Focus();
            // Capture touch so touch events are still pushed to CEF even if the touch leaves the control before a TouchUp.
            // This behavior is similar to how other browsers handle touch input.
            CaptureTouch(e.TouchDevice);
            OnTouch(e);
            base.OnTouchDown(e);
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.TouchMove" /> routed event that occurs when a touch moves while inside this element.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.TouchEventArgs" /> that contains the event data.</param>
        protected override void OnTouchMove(TouchEventArgs e)
        {
            OnTouch(e);
            base.OnTouchMove(e);
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.TouchUp" /> routed event that occurs when a touch is released inside this element.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.TouchEventArgs" /> that contains the event data.</param>
        protected override void OnTouchUp(TouchEventArgs e)
        {
            ReleaseTouchCapture(e.TouchDevice);
            OnTouch(e);
            base.OnTouchUp(e);
        }

        /// <summary>
        /// Handles a <see cref="E:Touch" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TouchEventArgs"/> instance containing the event data.</param>
        private void OnTouch(TouchEventArgs e)
        {
            var browser = GetBrowser();

            if (!e.Handled && browser != null)
            {
                var modifiers = WpfExtensions.GetModifierKeys();
                var touchPoint = e.GetTouchPoint(this);
                var touchEventType = TouchEventType.Cancelled;
                switch (touchPoint.Action)
                {
                    case TouchAction.Down:
                    {
                        touchEventType = TouchEventType.Pressed;
                        break;
                    }
                    case TouchAction.Move:
                    {
                        touchEventType = TouchEventType.Moved;
                        break;
                    }
                    case TouchAction.Up:
                    {
                        touchEventType = TouchEventType.Released;
                        break;
                    }
                    default:
                    {
                        touchEventType = TouchEventType.Cancelled;
                        break;
                    }
                }

                var touchEvent = new TouchEvent()
                {
                    Id = e.TouchDevice.Id,
                    X = (float)touchPoint.Position.X,
                    Y = (float)touchPoint.Position.Y,
                    PointerType = PointerType.Touch,
                    Type = touchEventType,
                    Modifiers = modifiers,
                };

                browser.GetHost().SendTouchEvent(touchEvent);

                e.Handled = true;
            }
        }

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

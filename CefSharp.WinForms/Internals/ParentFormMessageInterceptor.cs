// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp.Internals;

namespace CefSharp.WinForms.Internals
{
    internal class ParentFormMessageInterceptor : NativeWindow, IDisposable
    {
        /// <summary>
        /// Keep track of whether a move is in progress.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Used to determine the coordinates involved in the move
        /// </summary>
        private Rectangle movingRectangle;

        private ChromiumWebBrowser Browser { get; set; }

        private Form ParentForm { get; set; }

        public ParentFormMessageInterceptor(ChromiumWebBrowser browser)
        {
            Browser = browser;
            // Get notified if our browser window parent changes:
            Browser.ParentChanged += ParentParentChanged;
            // Find the browser form to subclass to monitor WM_MOVE/WM_MOVING
            RefindParentForm();
        }

        /// <summary>
        /// Call to force refinding of the parent Form. 
        /// (i.e. top level window that owns the ChromiumWebBrowserControl)
        /// </summary>
        public void RefindParentForm()
        {
            ParentParentChanged(Browser, null);
        }

        /// <summary>
        /// Adjust the form to listen to if the ChromiumWebBrowserControl's parent changes.
        /// </summary>
        /// <param name="sender">The ChromiumWebBrowser whose parent has changed.</param>
        /// <param name="e"></param>
        private void ParentParentChanged(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var oldForm = ParentForm;
            var newForm = control.FindForm();

            if (oldForm == null || newForm == null || oldForm.Handle != newForm.Handle)
            {
                if (Handle != IntPtr.Zero)
                {
                    ReleaseHandle();
                }
                if (oldForm != null)
                {
                    oldForm.HandleCreated -= OnHandleCreated;
                    oldForm.HandleDestroyed -= OnHandleDestroyed;
                }
                ParentForm = newForm;
                if (newForm != null)
                {
                    newForm.HandleCreated += OnHandleCreated;
                    newForm.HandleDestroyed += OnHandleDestroyed;
                    // If newForm's Handle has been created already,
                    // our event listener won't be called, so call it now.
                    if (newForm.IsHandleCreated)
                    {
                        OnHandleCreated(newForm, null);
                    }
                }
            }
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            AssignHandle(((Form)sender).Handle);
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            var isMovingMessage = false;

            // Negative initial values keeps the compiler quiet and to
            // ensure we have actual window movement to notify CEF about.
            const int invalidMoveCoordinate = -1;
            var x = invalidMoveCoordinate;
            var y = invalidMoveCoordinate;

            // Listen for operating system messages 
            switch (m.Msg)
            {
                case NativeMethods.WM_ACTIVATE:
                {
                    // Intercept (de)activate messages for our form so that we can
                    // ensure that we play nicely with WinForms .ActiveControl
                    // tracking.
                    var browser = Browser;
                    if ((int)m.WParam == 0x0) // WA_INACTIVE
                    {
                        // If the CEF browser no longer has focus,
                        // we won't get a call to OnLostFocus on ChromiumWebBrowser.
                        // However, it doesn't matter so much since the CEF
                        // browser will receive it instead.

                        // Paranoia about making sure the IsActivating state is correct now.
                        browser.IsActivating = false;
                        DefWndProc(ref m);
                    }
                    else // WA_ACTIVE or WA_CLICKACTIVE
                    {
                        // Only set IsActivating if the ChromiumWebBrowser was the form's
                        // ActiveControl before the last deactivation.
                        browser.IsActivating = browser.IsActiveControl();
                        DefWndProc(ref m);
                        // During activation, WM_SETFOCUS gets sent to
                        // to the CEF control since it's the root window
                        // of the CEF UI thread.
                        //
                        // Therefore, don't set .IsActivating to false here
                        // instead do so in DefaultFocusHandler.OnGotFocus.
                        // Otherwise there's a race condition between this
                        // thread setting activating to false and
                        // the CEF DefaultFocusHandler executing to determine
                        // it shouldn't Activate() the control.
                    }
                    return;
                }
                case NativeMethods.WM_MOVING:
                {
                    movingRectangle = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
                    x = movingRectangle.Left;
                    y = movingRectangle.Top;
                    isMovingMessage = true;
                    break;
                }
                case NativeMethods.WM_MOVE:
                {
                    // Convert IntPtr into 32bit int safely without 
                    // exceptions:
                    int dwLParam = m.LParam.CastToInt32();

                    // Extract coordinates from lo/hi word:
                    x = dwLParam & 0xffff;
                    y = (dwLParam >> 16) & 0xffff;

                    isMovingMessage = true;
                    break;
                }
            }

            // Only notify about movement if:
            // * Browser Handle Created
            //   NOTE: This is checked for paranoia. 
            //         This WndProc can't be called unless ParentForm has 
            //         its handle created, but that doesn't necessarily mean 
            //         Browser has had its handle created.
            //         WinForm controls don't usually get eagerly created Handles
            //         in their constructors.
            // * ParentForm Actually moved
            // * Not currently moving (on the UI thread only of course)
            // * The current WindowState is Normal.
            //      This check is to simplify the effort here.
            //      Other logic already handles the maximize/minimize
            //      cases just fine.
            // You might consider checking Browser.Visible and
            // not notifying our browser control if the browser control isn't visible.
            // However, if you do that, the non-Visible CEF tab will still
            // have any SELECT drop downs rendering where they shouldn't.
            if (isMovingMessage
                && Browser.IsHandleCreated
                && ParentForm.WindowState == FormWindowState.Normal
                && (ParentForm.Left != x || ParentForm.Top != y)
                && !isMoving)
            {
                // ParentForm.Left & .Right are negative when the window
                // is transitioning from maximized to normal.
                // If we are transitioning, the form will also receive
                // a WM_SIZE which can deal with the move/size combo itself.
                if (ParentForm.Left >= 0 && ParentForm.Right >= 0)
                {
                    OnMoving();
                }
            }
            DefWndProc(ref m);
        }

        protected virtual void OnMoving()
        {
            isMoving = true;

            if (Browser.IsBrowserInitialized)
            {
                Browser.NotifyMoveOrResizeStarted();
            }

            isMoving = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ParentForm != null)
                {
                    ParentForm.HandleCreated -= OnHandleCreated;
                    ParentForm.HandleDestroyed -= OnHandleDestroyed;
                    ParentForm = null;
                }

                // Unmanaged resource, but release here anyway.
                // NativeWindow has its own finalization logic 
                // that should be run if this instance isn't disposed
                // properly before arriving at the finalization thread.
                // See: http://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/NativeWindow.cs,147
                // for the gruesome details.
                if (Handle != IntPtr.Zero)
                {
                    ReleaseHandle();
                }

                if (Browser != null)
                {
                    Browser.ParentChanged -= ParentParentChanged;
                    Browser = null;
                }
            }
        }

        protected override void OnThreadException(Exception e)
        {
            // TODO: Do something more interesting here, logging, whatever, something.
            base.OnThreadException(e);
        }
    }
}

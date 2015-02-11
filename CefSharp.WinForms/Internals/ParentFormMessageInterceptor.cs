// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    internal class ParentFormMessageInterceptor : NativeWindow, IDisposable
    {
        public event EventHandler<EventArgs> Moving;

        private bool isMoving = false;
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
            Control control = (Control)sender;
            Form oldForm = ParentForm;
            Form newForm = control.FindForm();
            if (oldForm == null
                || newForm == null
                || oldForm.Handle != newForm.Handle
                )
            {
                if (this.Handle != IntPtr.Zero)
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
            bool isMovingMessage = false;

            // Negative initial values keeps the compiler quiet and to
            // ensure we have actual window movement to notify CEF about.
            const int invalidMoveCoordinate = -1;
            int x = invalidMoveCoordinate;
            int y = invalidMoveCoordinate;

            // Listen for operating system messages 
            switch (m.Msg)
            {
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
                    x = (m.LParam.ToInt32() & 0xffff);
                    y = ((m.LParam.ToInt32() >> 16) & 0xffff);
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
                && (ParentForm.Left != x
                    || ParentForm.Top != y)
                && !isMoving)
            {
                // ParentForm.Left & .Right are negative when the window
                // is transitioning from maximized to normal.
                // If we are transitioning, the form will also receive
                // a WM_SIZE which can deal with the move/size combo itself.
                if (ParentForm.Left >= 0
                    && ParentForm.Right >= 0)
                {
                    OnMoving();
                }
            }
            this.DefWndProc(ref m);
        }

        protected virtual void OnMoving()
        {
            isMoving = true;
            var handler = Moving;

            if (handler != null)
            {
                handler(this, null);
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
                if (this.Handle != IntPtr.Zero)
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

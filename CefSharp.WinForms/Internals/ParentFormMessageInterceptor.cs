// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp.Internals;

namespace CefSharp.WinForms.Internals
{
    /// <summary>
    /// ParentFormMessageInterceptor - hooks into the parent forms
    /// message loop to incercept messages like WM_MOVE
    /// </summary>
    /// <seealso cref="System.Windows.Forms.NativeWindow" />
    /// <seealso cref="System.IDisposable" />
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

        /// <summary>
        /// Store the previous window state, used to determine if the
        /// Windows was previously <see cref="FormWindowState.Minimized"/>
        /// and resume rendering
        /// </summary>
        private FormWindowState previousWindowState;

        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        /// <value>The browser.</value>
        private ChromiumWebBrowser Browser { get; set; }

        /// <summary>
        /// Gets or sets the parent form.
        /// </summary>
        /// <value>The parent form.</value>
        private Form ParentForm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentFormMessageInterceptor"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
                    oldForm.Resize -= OnResize;
                }
                ParentForm = newForm;
                if (newForm != null)
                {
                    newForm.HandleCreated += OnHandleCreated;
                    newForm.HandleDestroyed += OnHandleDestroyed;
                    newForm.Resize += OnResize;

                    previousWindowState = newForm.WindowState;

                    // If newForm's Handle has been created already,
                    // our event listener won't be called, so call it now.
                    if (newForm.IsHandleCreated)
                    {
                        OnHandleCreated(newForm, null);
                    }
                }
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            var form = (Form)sender;

            if (previousWindowState == form.WindowState)
            {
                return;
            }

            switch (form.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                {
                    if (previousWindowState == FormWindowState.Minimized)
                    {
                        Browser?.ShowInternal();
                    }
                    break;
                }
                case FormWindowState.Minimized:
                {
                    Browser?.HideInternal();

                    break;
                }
            }

            previousWindowState = form.WindowState;
        }

        /// <summary>
        /// Handles the <see cref="E:HandleCreated" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnHandleCreated(object sender, EventArgs e)
        {
            AssignHandle(((Form)sender).Handle);
        }

        /// <summary>
        /// Handles the <see cref="E:HandleDestroyed" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message.</param>
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

        /// <summary>
        /// Called when [moving].
        /// </summary>
        protected virtual void OnMoving()
        {
            isMoving = true;

            if (Browser.IsBrowserInitialized)
            {
                Browser.GetBrowser().GetHost().NotifyMoveOrResizeStarted();
            }

            isMoving = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ParentForm != null)
                {
                    ParentForm.HandleCreated -= OnHandleCreated;
                    ParentForm.HandleDestroyed -= OnHandleDestroyed;
                    ParentForm.Resize -= OnResize;
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

        /// <summary>
        /// When overridden in a derived class, manages an unhandled thread exception.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Exception" /> that specifies the unhandled thread exception.</param>
        protected override void OnThreadException(Exception e)
        {
            // TODO: Do something more interesting here, logging, whatever, something.
            base.OnThreadException(e);
        }
    }
}

// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Helper
{
    /// <summary>
    /// When using ILifeSpanHandler.OnBeforePopup and calling IWindowInfo.SetAsChild
    /// it's important to track form movement and control size change. This NativeWindow
    /// does that for you. With newer version it's possible this is only required when using
    /// MultiThreadedMessageLoop = true.
    /// TODO: Currently there's a 1:1 mapping between helpers and browsers, it should be possible
    /// to have one helper per form that notifies all browser instances 
    /// </summary>
    public class PopupAsChildHelper : NativeWindow, IDisposable
    {
        /// <summary>
        /// Keep track of whether a move is in progress.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Gets or sets the parent control used to host the popup child handle
        /// </summary>
        private Control parentControl;

        /// <summary>
        /// Gets or sets the parent form.
        /// </summary>
        /// <value>The parent form.</value>
        private Form parentForm;

        /// <summary>
        /// The IBrowser that references the Popup
        /// </summary>
        private IBrowser browser;

        /// <summary>
        /// The browsers window handle(hwnd)
        /// </summary>
        private readonly IntPtr browserHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentFormMessageInterceptor"/> class.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public PopupAsChildHelper(IBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser");
            }

            this.browser = browser;

            //From the browser we grab the window handle (hwnd)
            this.browserHandle = browser.GetHost().GetWindowHandle();

            //WinForms will kindly lookup the child control from it's handle
            this.parentControl = Control.FromChildHandle(browserHandle);

            if (this.parentControl == null)
            {
                throw new Exception("Unable to locate parentControl from the browser handle.");
            }

            // Get notified if our control window parent changes:
            this.parentControl.ParentChanged += ParentFormChanged;
            //Get notified of size changes
            this.parentControl.SizeChanged += ParentControlSizeChanged;

            // Find the browser form to subclass to monitor WM_MOVE/WM_MOVING
            RefindParentForm();
        }

        /// <summary>
        /// Call to force refinding of the parent Form.
        /// (i.e. top level window that owns the ChromiumWebBrowserControl)
        /// </summary>
        public void RefindParentForm()
        {
            parentControl.InvokeOnUiThreadIfRequired(() =>
            {
                ParentFormChanged(parentControl, null);
            });
        }

        /// <summary>
        /// Adjust the form to listen to if the ChromiumWebBrowserControl's parent changes.
        /// </summary>
        /// <param name="sender">The ChromiumWebBrowser whose parent has changed.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ParentFormChanged(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var oldForm = parentForm;
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
                parentForm = newForm;
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

        private void ParentControlSizeChanged(object sender, EventArgs e)
        {
            var bounds = parentControl.Bounds;
            if (browserHandle != IntPtr.Zero)
            {
                NativeMethodWrapper.SetWindowPosition(browserHandle, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }
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
                case 0x216: //WM_MOVING
                {
                    var movingRectangle = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
                    x = movingRectangle.Left;
                    y = movingRectangle.Top;
                    isMovingMessage = true;
                    break;
                }
                case 0x3: //WM_MOVE
                {
                    // Convert IntPtr into 32bit int safely without 
                    // exceptions:
                    int dwLParam = CastToInt32(m.LParam);

                    // Extract coordinates from lo/hi word:
                    x = dwLParam & 0xffff;
                    y = (dwLParam >> 16) & 0xffff;

                    isMovingMessage = true;
                    break;
                }
            }

            // Only notify about movement if:
            // * ParentControl Handle Created
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
            // You might consider checking Control.Visible and
            // not notifying our browser control if the browser control isn't visible.
            // However, if you do that, the non-Visible CEF tab will still
            // have any SELECT drop downs rendering where they shouldn't.
            if (isMovingMessage
                && parentControl.IsHandleCreated
                && parentForm.WindowState == FormWindowState.Normal
                && (parentForm.Left != x || parentForm.Top != y)
                && !isMoving)
            {
                // parentForm.Left & .Right are negative when the window
                // is transitioning from maximized to normal.
                // If we are transitioning, the form will also receive
                // a WM_SIZE which can deal with the move/size combo itself.
                if (parentForm.Left >= 0 && parentForm.Right >= 0)
                {
                    OnMoving();
                }
            }

            DefWndProc(ref m);
        }

        /// <summary>
        /// Called when the window is moving.
        /// </summary>
        protected virtual void OnMoving()
        {
            isMoving = true;

            if (!browser.IsDisposed)
            {
                browser.GetHost().NotifyMoveOrResizeStarted();
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
                if (parentForm != null)
                {
                    parentForm.HandleCreated -= OnHandleCreated;
                    parentForm.HandleDestroyed -= OnHandleDestroyed;
                    parentForm = null;
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

                if (parentControl != null)
                {
                    parentControl.ParentChanged -= ParentFormChanged;
                    parentControl.SizeChanged -= ParentControlSizeChanged;
                    parentControl = null;
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

        private static int CastToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
    }
}

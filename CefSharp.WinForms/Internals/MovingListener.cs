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
    internal class MovingListener : NativeWindow
    {
        public event EventHandler<EventArgs> Moving;

        private bool isMoving = false;
        private Rectangle movingRectangle;

        private Control ParentControl { get; set; }

        private Form ParentForm { get; set; }

        public MovingListener(Control parent)
        {
            ParentControl = parent;
            // Get notified if our parent window changes:
            ParentControl.ParentChanged += ParentParentChanged;
            // Find the parent form to subclass to monitor WM_MOVE/WM_MOVING
            RefindParentForm();
        }

        public void RefindParentForm()
        {
            ParentParentChanged(ParentControl, null);
        }

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
            Kernel32.OutputDebugString(String.Format("Attaching '{1:x}' to form '{0:x}'\r\n", ParentForm.Handle, ParentControl.Handle));
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            bool care = false;

            // Negative initial values keeps the compiler quiet and to
            // ensure we have actual window movement to notify CEF about.
            const int invalidMoveCoordinate = -1;
            int x = invalidMoveCoordinate;
            int y = invalidMoveCoordinate;

            // Listen for operating system messages 
            switch (m.Msg)
            {
            // WM_MOVING:
            case 0x0216:
                movingRectangle = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
                x = movingRectangle.Left;
                y = movingRectangle.Top;
                care = true;
                break;
            // WM_MOVE
            case 0x3:
                x = (m.LParam.ToInt32() & 0xffff);
                y = ((m.LParam.ToInt32() >> 16) & 0xffff);
                care = true;
                break;
            }

            // Only notify about movement if:
            // * ParentControl Handle Created
            //   NOTE: This is checked for paranoia. 
            //         This WndProc can't be called unless ParentForm has 
            //         its handle created, but that doesn't necessarily mean 
            //         ParentControl has had its handle created.
            //         WinForm controls don't usually get eagerly created Handles
            //         in their constructors.
            // * ParentForm Actually moved
            // * Not currently moving (on the UI thread only of course)
            // * The current WindowState is Normal.
            //      This check is to simplify the effort here.
            //      Other logic already handles the maximize/minimize
            //      cases just fine.
            // You might consider checking ParentControl.Visible and
            // not notifying our parent control if the parent control isn't visible.
            // However, if you do that, the non-Visible CEF tab will still
            // have any SELECT drop downs rendering where they shouldn't.
            if (care
                && ParentControl.IsHandleCreated
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
                    Kernel32.OutputDebugString(String.Format("Called OnMoving from control '{1:x}' to form '{0:x}'\r\n", ParentForm.Handle, ParentControl.Handle));
                }
                else
                {
                    Kernel32.OutputDebugString(String.Format("Skipped OnMoving from control '{1:x}' to form '{0:x}'\r\n", ParentForm.Handle, ParentControl.Handle));
                }
            }
            else if (care)
            {
                Kernel32.OutputDebugString(String.Format("Skipped OnMoving from control '{1:x}' to form '{0:x}'\r\n", ParentForm.Handle, ParentControl.Handle));
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
    }
}

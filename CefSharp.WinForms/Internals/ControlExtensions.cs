// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public static void Activate(this Control control)
        {
            //Notify WinForms world that inner browser window got focus. This will trigger Leave event to previous focused control
            var containerControl = control.GetContainerControl();
            if (containerControl != null)
            {
                containerControl.ActivateControl(control);
            }
        }

        public static void SelectNextControl(this Control control, bool next)
        {
            var containerControl = control.GetContainerControl();

            while (containerControl != null)
            {
                var containerControlAsControl = containerControl as Control;
                if (containerControlAsControl == null)
                {
                    break;
                }

                var activeControl = containerControl.ActiveControl;
                if (containerControlAsControl.SelectNextControl(activeControl, next, true, true, false))
                {
                    break;
                }

                if (containerControlAsControl.Parent == null)
                {
                    break;
                }

                containerControl = containerControlAsControl.Parent.GetContainerControl();
            }
        }
    }
}

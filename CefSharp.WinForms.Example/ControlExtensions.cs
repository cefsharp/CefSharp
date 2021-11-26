// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Finds the parent for the given control of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="control">control</param>
        /// <returns>Parent or null</returns>
        public static T GetParentOfType<T>(this Control control) where T : Control
        {
            if(control.IsDisposed  || !control.IsHandleCreated)
            {
                return default;
            }

            var current = control;

            do
            {
                current = current.Parent;

                if (current == null)
                {
                    return default;
                }

            }
            while (current.GetType() != typeof(T));

            return (T)current;
        }
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// No action will be performed if the control doesn't have a valid handle or the control is Disposed/Disposing.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            //See https://stackoverflow.com/questions/1874728/avoid-calling-invoke-when-the-control-is-disposed
            //for background and some guidance when implementing your own version.
            //No action
            if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke((Action)(() =>
                {
                    //No action
                    if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
                    {
                        return;
                    }

                    action();
                }));
            }
            else
            {
                action.Invoke();
            }
        }
    }
}

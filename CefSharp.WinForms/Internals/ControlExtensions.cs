using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    public static class ControlExtensions
    {
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

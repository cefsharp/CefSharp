using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    internal class MovingListener : NativeWindow
    {
        public event EventHandler<EventArgs> Moving;

        private Form ParentForm { get; set; }

        public MovingListener(Form parent)
        {
            ParentForm = parent;
            if (ParentForm.IsHandleCreated)
            {
                OnHandleCreated(ParentForm, null);
            }
            else
            {
                ParentForm.HandleCreated += OnHandleCreated;
            }
            ParentForm.HandleDestroyed += OnHandleDestroyed;
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
            // Listen for operating system messages 
            switch (m.Msg)
            {
            // WM_MOVE
            case 0x3:
            // WM_MOVING:
            case 0x0216:
                OnMoving();
                break;
            }
            base.WndProc(ref m);
        }

        protected virtual void OnMoving()
        {
            if (Moving != null)
            {
                Moving(this, null);
            }
        }
    }
}

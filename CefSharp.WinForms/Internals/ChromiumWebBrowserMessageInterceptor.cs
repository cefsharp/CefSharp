using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    public class ChromiumWebBrowserMessageInterceptor : NativeWindow, IDisposable
    {
        private Action<Message> onMessageCallback;

        public ChromiumWebBrowserMessageInterceptor(Action<Message> callback)
        {
            onMessageCallback = callback;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            onMessageCallback(m);
        }

        public void Dispose()
        {
            ReleaseHandle();
            onMessageCallback = null;
        }
    }
}

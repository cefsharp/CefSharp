using CefSharp.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms.Internals
{
    public class ChromiumWebBrowserMessageInterceptor : NativeWindow, IDisposable
    {
        private ChromiumWebBrowser webBrowser;
        private Action<Message> onMessageCallback;

        public ChromiumWebBrowserMessageInterceptor(ChromiumWebBrowser browser, Action<Message> callback)
        {
            webBrowser = browser;
            onMessageCallback = callback;

            if (browser.IsBrowserInitialized)
            {
                AssignHandle();
            }
            else
            {
                browser.IsBrowserInitializedChanged += browser_IsBrowserInitializedChanged;
            }
        }

        private void AssignHandle()
        {
            List<IntPtr> childWindows = new List<IntPtr>();
            User32.GetSubWindows(webBrowser.Handle, childWindows);
            var cefWindowHandle = childWindows.Last();
            AssignHandle(cefWindowHandle);
        }

        private void browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            webBrowser.IsBrowserInitializedChanged -= browser_IsBrowserInitializedChanged;
            webBrowser.Invoke(new MethodInvoker(AssignHandle));
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            onMessageCallback(m);
        }

        public void Dispose()
        {
            ReleaseHandle();
            webBrowser = null;
            onMessageCallback = null;
        }
    }
}

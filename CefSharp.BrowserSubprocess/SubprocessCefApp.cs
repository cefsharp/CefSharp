using CefSharp.Wrappers;

namespace CefSharp.BrowserSubprocess
{
    internal class SubprocessCefApp : CefAppWrapper
    {
        private static SubprocessCefApp instance;

        public static SubprocessCefApp Instance
        {
            get { return instance ?? (instance = new SubprocessCefApp()); }
        }

        public CefBrowserWrapper CefBrowserWrapper { get; private set; }

        public override void OnBrowserCreated(CefBrowserWrapper cefBrowserWrapper)
        {
            CefBrowserWrapper = cefBrowserWrapper;
            JavascriptServiceHost.Create(cefBrowserWrapper.BrowserId);
        }
    }
}
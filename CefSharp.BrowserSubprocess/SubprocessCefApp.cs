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

        public CefSubprocessWrapper CefSubprocessWrapper { get; private set; }
        public int? ParentProcessId { get; set; }

        public override void OnBrowserCreated(CefSubprocessWrapper cefBrowserWrapper)
        {
            CefSubprocessWrapper = cefBrowserWrapper;

            if (ParentProcessId != null)
            {
                JavascriptServiceHost.Create(ParentProcessId.Value, CefSubprocessWrapper.BrowserId);
            }
        }
    }
}
using CefSharp.Wrappers;
using System.ServiceModel;
using System.Threading;

namespace CefSharp.BrowserSubprocess
{
    internal class SubprocessCefApp : CefAppWrapper
    {
        #region Singleton pattern

        private static SubprocessCefApp instance;

        public static SubprocessCefApp Instance
        {
            get { return instance ?? (instance = new SubprocessCefApp()); }
        }

        private SubprocessCefApp()
        {            
        }

        #endregion

        private ServiceHost javascriptServiceHost;

        public CefSubprocessWrapper CefSubprocessWrapper { get; private set; }
        public int? ParentProcessId { get; set; }

        public override void OnBrowserCreated(CefSubprocessWrapper cefBrowserWrapper)
        {
            CefSubprocessWrapper = cefBrowserWrapper;

            if (ParentProcessId == null)
            {
                return;
            }

            var thread = new Thread((Action) => javascriptServiceHost = JavascriptServiceHost.Create(ParentProcessId.Value, CefSubprocessWrapper.BrowserId));
            thread.Start();
        }

        public void TerminateJavascriptServiceHost()
        {
            if (javascriptServiceHost != null)
            {
                javascriptServiceHost.Close();
            }
        }
    }
}
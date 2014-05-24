using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderprocess : CefSubprocess
    {
        private SubprocessServiceHost javascriptServiceHost;
        private CefBrowserBase browser;

        public CefBrowserBase Browser
        {
            get { return browser; }
        }

        public SubprocessServiceHost ServiceHost
        {
            get { return javascriptServiceHost; }
        }

        public CefRenderprocess(IEnumerable<string> args) 
            : base(args)
        {
        }

        public static new CefRenderprocess Instance
        {
            get { return (CefRenderprocess)CefSubprocess.Instance; }
        }



        protected override void DoDispose(bool isDisposing)
        {
            DisposeMember(ref javascriptServiceHost);
            DisposeMember(ref browser);

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserBase cefBrowserWrapper)
        {
            browser = cefBrowserWrapper;

            if (ParentProcessId == null)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                javascriptServiceHost = SubprocessServiceHost.Create(ParentProcessId.Value, cefBrowserWrapper.BrowserId);
                javascriptServiceHost.Initialized += (c) => Callback = c;
            });
        }
    }
}

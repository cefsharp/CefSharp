using System.Collections.Generic;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderProcess : CefSubProcess
    {
        private readonly List<CefBrowserWrapper> browsers = new List<CefBrowserWrapper>();
        private int? parentBrowserId;

        protected IEnumerable<CefBrowserWrapper> Browsers
        {
            get { return browsers.AsReadOnly(); }
        }

        protected int? ParentBrowserId
        {
            get { return parentBrowserId; }
        }

        public CefRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            foreach(var browser in browsers)
            {
                browser.Dispose();
            }

            browsers.Clear();

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserWrapper browser)
        {
            browsers.Add(browser);

            if (parentBrowserId == null)
            {
                parentBrowserId = browser.BrowserId;
            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            browsers.Remove(browser);
        }
    }
}

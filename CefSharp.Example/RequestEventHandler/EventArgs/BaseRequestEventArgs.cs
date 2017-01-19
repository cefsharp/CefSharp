namespace CefSharp.Example.RequestEventHandler {
    public abstract class BaseRequestEventArgs : System.EventArgs {
        protected BaseRequestEventArgs(IWebBrowser browserControl, IBrowser browser) {
            BrowserControl = browserControl;
            Browser = browser;
        }

        public IWebBrowser BrowserControl { get; }
        public IBrowser Browser { get; }
    }
}

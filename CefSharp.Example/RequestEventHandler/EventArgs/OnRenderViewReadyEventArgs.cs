namespace CefSharp.Example.RequestEventHandler {
    public class OnRenderViewReadyEventArgs : BaseRequestEventArgs {
        public OnRenderViewReadyEventArgs(IWebBrowser browserControl, IBrowser browser) : base(browserControl, browser) {}
    }
}

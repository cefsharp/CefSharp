namespace CefSharp.Example.RequestEventHandler {
    public class OnRenderProcessTerminatedEventArgs : BaseRequestEventArgs {
        public OnRenderProcessTerminatedEventArgs(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
            : base(browserControl, browser) {
            Status = status;
        }

        public CefTerminationStatus Status { get; }
    }
}

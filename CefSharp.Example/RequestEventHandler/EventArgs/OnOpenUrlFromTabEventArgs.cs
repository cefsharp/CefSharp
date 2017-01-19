namespace CefSharp.Example.RequestEventHandler {
    public class OnOpenUrlFromTabEventArgs : BaseRequestEventArgs {
        public OnOpenUrlFromTabEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
            : base(browserControl, browser) {
            Frame = frame;
            TargetUrl = targetUrl;
            TargetDisposition = targetDisposition;
            UserGesture = userGesture;
        }

        public IFrame Frame { get; }
        public string TargetUrl { get; }
        public WindowOpenDisposition TargetDisposition { get; }
        public bool UserGesture { get; }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; } = false;
    }
}

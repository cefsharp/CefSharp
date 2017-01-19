namespace CefSharp.Example.RequestEventHandler {
    public class OnProtocolExecutionEventArgs : BaseRequestEventArgs {
        public OnProtocolExecutionEventArgs(IWebBrowser browserControl, IBrowser browser, string url) : base(browserControl, browser) {
            Url = url;
        }

        public string Url { get; }

        /// <summary>
        ///     Set to true to attempt execution via the registered OS protocol handler, if any. Otherwise set to false.
        /// </summary>
        public bool AttemptExecution { get; set; } = false; //TODO discuss if the default should be true
    }
}

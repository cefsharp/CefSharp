namespace CefSharp.Example.RequestEventHandler
{
    public class OnQuotaRequestEventArgs : BaseRequestEventArgs
    {
        public OnQuotaRequestEventArgs(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            : base(browserControl, browser)
        {
            OriginUrl = originUrl;
            NewSize = newSize;
            Callback = callback;
        }

        public string OriginUrl { get; }
        public long NewSize { get; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true to continue the request
        ///     and call <see cref="M:CefSharp.IRequestCallback.Continue(System.Boolean)" /> either in this method or at a later
        ///     time to
        ///     grant or deny the request.
        /// </summary>
        public bool ContinueAsync { get; set; } = false;
    }
}

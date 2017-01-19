namespace CefSharp.Example.RequestEventHandler
{
    public class OnBeforeBrowseEventArgs : BaseRequestEventArgs
    {
        public OnBeforeBrowseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            IsRedirect = isRedirect;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }
        public bool IsRedirect { get; }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; } = false;
    }
}

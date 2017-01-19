namespace CefSharp.Example.RequestEventHandler
{
    public class OnBeforeResourceLoadEventArgs : BaseRequestEventArgs
    {
        public OnBeforeResourceLoadEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Callback = callback;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; }

        /// <summary>
        ///     To cancel loading of the resource return <see cref="F:CefSharp.CefReturnValue.Cancel" />
        ///     or <see cref="F:CefSharp.CefReturnValue.Continue" /> to allow the resource to load normally. For async
        ///     return <see cref="F:CefSharp.CefReturnValue.ContinueAsync" />
        /// </summary>
        public CefReturnValue ResourceLoadHandling { get; set; } = CefReturnValue.Continue;
    }
}

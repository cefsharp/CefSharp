namespace CefSharp.Example.RequestEventHandler
{
    /// <summary>
    ///     baijdhsaohg fdupih g
    /// </summary>
    public class OnResourceResponseEventArgs : BaseRequestEventArgs
    {
        public OnResourceResponseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }

        /// <summary>
        ///     To allow the resource to load normally set to false.
        ///     To redirect or retry the resource, modify <see cref="OnBeforeResourceLoadEventArgs.Request" /> (url, headers or
        ///     post body) and set to true.
        /// </summary>
        public bool RedirectOrRetry { get; set; } = false;
    }
}

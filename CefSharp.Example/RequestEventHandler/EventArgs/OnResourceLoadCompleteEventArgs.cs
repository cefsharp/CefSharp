namespace CefSharp.Example.RequestEventHandler {
    public class OnResourceLoadCompleteEventArgs : BaseRequestEventArgs {
        public OnResourceLoadCompleteEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            Response = response;
            Status = status;
            ReceivedContentLength = receivedContentLength;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }
        public UrlRequestStatus Status { get; }
        public long ReceivedContentLength { get; }
    }
}

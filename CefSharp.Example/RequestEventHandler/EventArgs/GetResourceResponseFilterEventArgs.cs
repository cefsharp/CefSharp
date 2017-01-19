namespace CefSharp.Example.RequestEventHandler {
    public class GetResourceResponseFilterEventArgs : BaseRequestEventArgs {
        public GetResourceResponseFilterEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)  : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            Response = response;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }

        /// <summary>
        ///     Set IResponseFilter to intercept this response, otherwise return null
        /// </summary>
        public IResponseFilter ResponseFilter { get; set; } = null;
    }
}

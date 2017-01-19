using System.Text;

namespace CefSharp.Example.RequestEventHandler {
    public class OnResourceRedirectEventArgs : BaseRequestEventArgs {
        public OnResourceRedirectEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, StringBuilder newUrl)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            NewUrl = newUrl;
        }

        public IFrame Frame { get; }
        public IRequest Request { get; }

        /// <summary>
        ///     the new URL and can be changed if desired
        /// </summary>
        public StringBuilder NewUrl { get; }
    }
}

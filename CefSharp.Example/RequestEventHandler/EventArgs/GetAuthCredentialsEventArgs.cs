namespace CefSharp.Example.RequestEventHandler
{
    public class GetAuthCredentialsEventArgs : BaseRequestEventArgs
    {
        public GetAuthCredentialsEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) : base(browserControl, browser)
        {
            Frame = frame;
            IsProxy = isProxy;
            Host = host;
            Port = port;
            Realm = realm;
            Scheme = scheme;
            Callback = callback;
        }

        public IFrame Frame { get; }
        public bool IsProxy { get; }
        public string Host { get; }
        public int Port { get; }
        public string Realm { get; }
        public string Scheme { get; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of authentication requests.
        /// </summary>
        public IAuthCallback Callback { get; }

        /// <summary>
        ///     Set to true to continue the request and call
        ///     <see cref="T:CefSharp.IAuthCallback.Continue(System.String, System.String)" /> when the authentication information
        ///     is available. Set to false to cancel the request.
        /// </summary>
        public bool ContinueAsync { get; set; } = false;
    }
}

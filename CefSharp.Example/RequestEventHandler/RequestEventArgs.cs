#region

using System;
using System.Text;
using CefSharp;

#endregion

namespace Boxcryptor.UI.WPF {
    public interface ICancelNavigationEventArgs {
        bool CancelNavigation { get; set; }
    }

    public interface IBrowserNavigationEventArgs {
        IWebBrowser BrowserControl { get; }
        IBrowser Browser { get; }
    }

    public abstract class RequestEventArgs : EventArgs, IBrowserNavigationEventArgs {
        protected RequestEventArgs(IWebBrowser browserControl, IBrowser browser) {
            BrowserControl = browserControl;
            Browser = browser;
        }

        public IWebBrowser BrowserControl { get; }
        public IBrowser Browser { get; }
    }

    public class OnBeforeBrowseEventArgs : RequestEventArgs, ICancelNavigationEventArgs {
        public IFrame Frame { get; }
        public IRequest Request { get; }
        public bool IsRedirect { get; }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; } = false;

        public OnBeforeBrowseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            IsRedirect = isRedirect;
        }
    }

    public class OnOpenUrlFromTabEventArgs : RequestEventArgs, ICancelNavigationEventArgs {
        public IFrame Frame { get; }
        public string TargetUrl { get; }
        public WindowOpenDisposition TargetDisposition { get; }
        public bool UserGesture { get; }

        public OnOpenUrlFromTabEventArgs(
            IWebBrowser browserControl,
            IBrowser browser,
            IFrame frame,
            string targetUrl,
            WindowOpenDisposition targetDisposition,
            bool userGesture) : base(browserControl, browser) {
            Frame = frame;
            TargetUrl = targetUrl;
            TargetDisposition = targetDisposition;
            UserGesture = userGesture;
        }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; } = false;
    }

    public class OnResourceLoadCompleteEventArgs : RequestEventArgs {
        public OnResourceLoadCompleteEventArgs(
            IWebBrowser browserControl,
            IBrowser browser,
            IFrame frame,
            IRequest request,
            IResponse response,
            UrlRequestStatus status,
            long receivedContentLength) : base(browserControl, browser) {}
    }

    public class GetResourceResponseFilterEventArgs : RequestEventArgs {
        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }

        /// <summary>
        ///     Set IResponseFilter to intercept this response, otherwise return null
        /// </summary>
        public IResponseFilter ResponseFilter { get; set; } = null;

        public GetResourceResponseFilterEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            Response = response;
        }
    }

    /// <summary>
    ///     baijdhsaohg fdupih g
    /// </summary>
    public class OnResourceResponseEventArgs : RequestEventArgs {
        public IFrame Frame { get; }
        public IRequest Request { get; }
        public IResponse Response { get; }

        /// <summary>
        ///     To allow the resource to load normally set to false.
        ///     To redirect or retry the resource, modify <see cref="OnBeforeResourceLoadEventArgs.Request" /> (url, headers or
        ///     post body) and set to true.
        /// </summary>
        public bool RedirectOrRetry { get; set; } = false;

        public OnResourceResponseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            Response = response;
        }
    }

    public class OnRenderViewReadyEventArgs : RequestEventArgs {
        public OnRenderViewReadyEventArgs(IWebBrowser browserControl, IBrowser browser) : base(browserControl, browser) {}
    }

    public class OnResourceRedirectEventArgs : RequestEventArgs {
        public IFrame Frame { get; }
        public IRequest Request { get; }

        /// <summary>
        ///     the new URL and can be changed if desired
        /// </summary>
        public StringBuilder NewUrl { get; }

        public OnResourceRedirectEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, StringBuilder newUrl)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            NewUrl = newUrl;
        }
    }

    public class OnProtocolExecutionEventArgs : RequestEventArgs {
        public string Url { get; }

        /// <summary>
        ///     Set to true to attempt execution via the registered OS protocol handler, if any. Otherwise set to false.
        /// </summary>
        public bool AttemptExecution { get; set; } = false; //TODO discuss if the default should be true

        public OnProtocolExecutionEventArgs(IWebBrowser browserControl, IBrowser browser, string url) : base(browserControl, browser) {
            Url = url;
        }
    }

    public class OnQuotaRequestEventArgs : RequestEventArgs {
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

        public OnQuotaRequestEventArgs(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            : base(browserControl, browser) {
            OriginUrl = originUrl;
            NewSize = newSize;
            Callback = callback;
        }
    }

    public class OnRenderProcessTerminatedEventArgs : RequestEventArgs {
        public CefTerminationStatus Status { get; }

        public OnRenderProcessTerminatedEventArgs(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
            : base(browserControl, browser) {
            Status = status;
        }
    }

    public class GetAuthCredentialsEventArgs : RequestEventArgs {
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

        public GetAuthCredentialsEventArgs(
            IWebBrowser browserControl,
            IBrowser browser,
            IFrame frame,
            bool isProxy,
            string host,
            int port,
            string realm,
            string scheme,
            IAuthCallback callback) : base(browserControl, browser) {
            Frame = frame;
            IsProxy = isProxy;
            Host = host;
            Port = port;
            Realm = realm;
            Scheme = scheme;
            Callback = callback;
        }
    }

    public class OnBeforeResourceLoadEventArgs : RequestEventArgs {
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

        public OnBeforeResourceLoadEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            : base(browserControl, browser) {
            Frame = frame;
            Request = request;
            Callback = callback;
        }
    }

    public class OnPluginCrashedEventArgs : RequestEventArgs {
        public string PluginPath { get; }

        public OnPluginCrashedEventArgs(IWebBrowser browserControl, IBrowser browser, string pluginPath) : base(browserControl, browser) {
            PluginPath = pluginPath;
        }
    }

    public class OnCertificateErrorEventArgs : RequestEventArgs {
        public CefErrorCode ErrorCode { get; }
        public string RequestUrl { get; }
        public ISslInfo SSLInfo { get; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        ///     If empty the error cannot be recovered from and the request will be canceled automatically.
        /// </summary>
        public IRequestCallback Callback { get; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true and use <see cref="T:CefSharp.IRequestCallback" /> to
        ///     execute in an async fashion.
        /// </summary>
        public bool ContinueAsync { get; set; } = false;

        public OnCertificateErrorEventArgs(
            IWebBrowser browserControl,
            IBrowser browser,
            CefErrorCode errorCode,
            string requestUrl,
            ISslInfo sslInfo,
            IRequestCallback callback) : base(browserControl, browser) {
            ErrorCode = errorCode;
            RequestUrl = requestUrl;
            SSLInfo = sslInfo;
            Callback = callback;
        }
    }
}

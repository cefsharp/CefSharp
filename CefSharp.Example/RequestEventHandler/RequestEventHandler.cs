using System;
using System.Text;

namespace CefSharp.Example.RequestEventHandler
{
    /// <summary>
    ///     To use this class, check <see cref="IRequestHandler" /> for more information about the event parameters.
    ///     Often you will find MANDATORY information on how to work with the parameters or which thread the call comes from.
    ///     Simply check out the interface' method the event was named by.
    ///     (e.g <see cref="RequestEventHandler.OnCertificateErrorEvent" /> corresponds to
    ///     <see cref="IRequestHandler.OnCertificateError" />)
    ///     inspired by:
    ///     https://github.com/cefsharp/CefSharp/blob/fa41529853b2527eb0468a507ab6c5bd0768eb59/CefSharp.Example/RequestHandler.cs
    /// </summary>
    public class RequestEventHandler : IRequestHandler
    {
        public event EventHandler<OnBeforeBrowseEventArgs> OnBeforeBrowseEvent;
        public event EventHandler<OnOpenUrlFromTabEventArgs> OnOpenUrlFromTabEvent;
        public event EventHandler<OnCertificateErrorEventArgs> OnCertificateErrorEvent;
        public event EventHandler<OnPluginCrashedEventArgs> OnPluginCrashedEvent;
        public event EventHandler<OnBeforeResourceLoadEventArgs> OnBeforeResourceLoadEvent;
        public event EventHandler<GetAuthCredentialsEventArgs> GetAuthCredentialsEvent;
        public event EventHandler<OnRenderProcessTerminatedEventArgs> OnRenderProcessTerminatedEvent;
        public event EventHandler<OnQuotaRequestEventArgs> OnQuotaRequestEvent;
        public event EventHandler<OnResourceRedirectEventArgs> OnResourceRedirectEvent;

        /// <summary>
        ///     SECURITY WARNING: YOU SHOULD USE THIS EVENT TO ENFORCE RESTRICTIONS BASED ON SCHEME, HOST OR OTHER URL ANALYSIS
        ///     BEFORE ALLOWING OS EXECUTION.
        /// </summary>
        public event EventHandler<OnProtocolExecutionEventArgs> OnProtocolExecutionEvent;

        public event EventHandler<OnRenderViewReadyEventArgs> OnRenderViewReadyEvent;
        public event EventHandler<OnResourceResponseEventArgs> OnResourceResponseEvent;
        public event EventHandler<GetResourceResponseFilterEventArgs> GetResourceResponseFilterEvent;
        public event EventHandler<OnResourceLoadCompleteEventArgs> OnResourceLoadCompleteEvent;

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            var args = new OnBeforeBrowseEventArgs(browserControl, browser, frame, request, isRedirect);
            OnBeforeBrowseEvent?.Invoke(this, args);
            return args.CancelNavigation;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            var args = new OnOpenUrlFromTabEventArgs(browserControl, browser, frame, targetUrl, targetDisposition, userGesture);
            OnOpenUrlFromTabEvent?.Invoke(this, args);
            return args.CancelNavigation;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            var args = new OnCertificateErrorEventArgs(browserControl, browser, errorCode, requestUrl, sslInfo, callback);
            OnCertificateErrorEvent?.Invoke(this, args);

            //TODO: check if disposing the callback is intended like that
            if (!args.Callback.IsDisposed)
            {
                args.Callback.Dispose();
            }
            return args.ContinueAsync;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            var args = new OnPluginCrashedEventArgs(browserControl, browser, pluginPath);
            OnPluginCrashedEvent?.Invoke(this, args);
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            var args = new OnBeforeResourceLoadEventArgs(browserControl, browser, frame, request, callback);
            OnBeforeResourceLoadEvent?.Invoke(this, args);

            if (!args.Callback.IsDisposed)
            {
                args.Callback.Dispose();
            }
            return args.ResourceLoadHandling;
        }

        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            var args = new GetAuthCredentialsEventArgs(browserControl, browser, frame, isProxy, host, port, realm, scheme, callback);
            GetAuthCredentialsEvent?.Invoke(this, args);

            if (!args.Callback.IsDisposed)
            {
                args.Callback.Dispose();
            }

            return args.ContinueAsync;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            var args = new OnRenderProcessTerminatedEventArgs(browserControl, browser, status);
            OnRenderProcessTerminatedEvent?.Invoke(this, args);
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            var args = new OnQuotaRequestEventArgs(browserControl, browser, originUrl, newSize, callback);
            OnQuotaRequestEvent?.Invoke(this, args);

            if (!args.Callback.IsDisposed)
            {
                args.Callback.Dispose();
            }
            return args.ContinueAsync;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
            var args = new OnResourceRedirectEventArgs(browserControl, browser, frame, request, new StringBuilder(newUrl));
            OnResourceRedirectEvent?.Invoke(this, args);
            //TODO: test if the string is really changed by using a StringBuilder (should work afaik ;) )
            if (!Equals(newUrl, args.NewUrl.ToString()))
            {
                newUrl = args.NewUrl.ToString();
            }
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            var args = new OnProtocolExecutionEventArgs(browserControl, browser, url);
            OnProtocolExecutionEvent?.Invoke(this, args);
            return args.AttemptExecution;
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
            var args = new OnRenderViewReadyEventArgs(browserControl, browser);
            OnRenderViewReadyEvent?.Invoke(this, args);
        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new OnResourceResponseEventArgs(browserControl, browser, frame, request, response);
            OnResourceResponseEvent?.Invoke(this, args);
            return args.RedirectOrRetry;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var args = new GetResourceResponseFilterEventArgs(browserControl, browser, frame, request, response);
            GetResourceResponseFilterEvent?.Invoke(this, args);
            return args.ResponseFilter;
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var args = new OnResourceLoadCompleteEventArgs(browserControl, browser, frame, request, response, status, receivedContentLength);
            OnResourceLoadCompleteEvent?.Invoke(this, args);
        }
    }
}

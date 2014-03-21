using System.Net;

namespace CefSharp
{
    public enum NavigationType
    {
        LinkClicked,
        FormSubmitted,
        BackForward,
        Reload,
        FormResubmitted,
        Other
    };

    public interface IRequestHandler
    {
        bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType navigationType, bool isRedirect);
        bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse);
        void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers);
        bool GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler);
        bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password);

    }
}

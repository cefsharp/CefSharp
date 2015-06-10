using System;
using System.Collections.Generic;

namespace CefSharp.Example
{
    public class RequestHandler : IRequestHandler
    {
        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect, IFrame frame)
        {
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, IRequestCallback callback)
        {
            //To allow certificate
            //callback.Continue(true);
            //callback.Dispose();
            //return true;

            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //Note to Redirect simply set the request Url
            //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase))
            //{
            //    request.Url = "https://github.com/";
            //}

            //Callback in async fashion
            //callback.Continue(true);
            //callback.Dispose();
            //return CefReturnValue.ContinueAsync;

            return CefReturnValue.Continue;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browserControl, IBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            bool blockPluginLoad = false;

            // Enable next line to demo: Block any plugin with "flash" in its name
            // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y
            //blockPluginLoad = info.Name.ToLower().Contains("flash");

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return blockPluginLoad;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            //Accept Request to raise Quota
            //callback.Continue(true);
            //callback.Dispose();
            //return true;

            return false;
        }

        public void OnResourceRedirect(IWebBrowser browser, IFrame frame, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (string.Equals(frame.GetUrl(), "https://www.google.com/", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //	newUrl = "https://github.com";
            //}
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return url.StartsWith("mailto");
        }

        public void OnFaviconUrlChange(IWebBrowser browser, IList<string> urls)
        {
            var url = urls[0];
        }
    }
}

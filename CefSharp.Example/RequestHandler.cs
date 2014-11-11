using System;
using System.IO;
using System.Text;

namespace CefSharp.Example
{
    public class RequestHandler : IRequestHandler
    {
        private static readonly Uri ResourceUrl = new Uri("http://test/resource/load");

        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith(ResourceUrl.ToString()))
            {
                Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                requestResponse.RespondWith(resourceStream, "text/html");
            }

            return false;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, IWebPluginInfo info)
        {
            bool blockPluginLoad = false;

            // Enable next line to demo: Block any plugin with "flash" in its name
            // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y
            //blockPluginLoad = info.Name.ToLower().Contains("flash");
            return blockPluginLoad;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
        }
    }
}

﻿using System;
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

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            // here you could do fun stuff when a plugin crashes
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

        bool IRequestHandler.GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler)
        {
            handler = new DownloadHandler();

            return true;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policy_url, IWebPluginInfo info)
        {
            bool blockPluginLoad = false;

            // Enable next line to demo: Block any plugin with "flash" in its name
            // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y 
            //blockPluginLoad = info.Name.ToLower().Contains("flash");
            return blockPluginLoad;
        }
    }
}

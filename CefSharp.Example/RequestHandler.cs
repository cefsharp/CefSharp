// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example
{
    public class RequestHandler : IRequestHandler
    {
        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            return false;
        }

        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            try
            {
                //To allow certificate
                //callback.Continue(true);
                //return true;

                return false;
            }
            finally
            {
                callback.Dispose();
            }
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            using (callback)
            {
                if (request.Method == "POST")
                {
                    using (var postData = request.PostData)
                    {
                        var elements = postData.Elements;

                        var charSet = request.GetCharSet();

                        foreach (var element in elements)
                        {
                            if (element.Type == PostDataElementType.Bytes)
                            {
                                var body = element.GetBody(charSet);
                            }
                        }
                    }
                }

                //Note to Redirect simply set the request Url
                //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase))
                //{
                //    request.Url = "https://github.com/";
                //}

                //Callback in async fashion
                //callback.Continue(true);
                //return CefReturnValue.ContinueAsync;
            }
            
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

        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            try
            {
                //Accept Request to raise Quota
                //callback.Continue(true);
                //return true;

                return false;
            }
            finally
            {
                callback.Dispose();
            }
        }

        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (string.Equals(frame.GetUrl(), "https://www.google.com/", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //	newUrl = "https://github.com";
            //}
        }

        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return url.StartsWith("mailto");
        }

        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
            
        }
    }
}

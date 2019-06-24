// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Security.Cryptography.X509Certificates;
using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
    /// <summary>
    /// <see cref="RequestHandler"/> provides a base class for you to inherit from 
    /// you only need to implement the methods that are relevant to you. 
    /// If you implement the IRequestHandler interface you will need to
    /// implement every method
    /// </summary>
    public class ExampleRequestHandler : RequestHandler
    {
        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        protected override bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        protected override bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        protected override bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //To allow certificate
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        protected override void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        protected override bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            callback.Dispose();
            return false;
        }

        protected override bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            callback.Dispose();
            return false;
        }

        protected override void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
            browserControl.Load(CefExample.RenderProcessCrashedUrl);
        }

        protected override bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //Accept Request to raise Quota
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //NOTE: In most cases you examine the request.Url and only handle requests you are interested in
            if (request.Url.ToLower().StartsWith("https://cefsharp.example") || request.Url.ToLower().StartsWith("https://googlechrome.github.io/samples/service-worker/"))
            {
                return new ExampleResourceRequestHandler();
            }

            return null;
        }
    }
}

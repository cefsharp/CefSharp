// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
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

        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }

        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: We also suggest you wrap callback in a using statement or explicitly execute callback.Dispose as callback wraps an unmanaged resource.

            //Example #1
            //Return true and call IRequestCallback.Continue() at a later time to continue or cancel the request.
            //In this instance we'll use a Task, typically you'd invoke a call to the UI Thread and display a Dialog to the user
            //You can cast the IWebBrowser param to ChromiumWebBrowser to easily access
            //control, from there you can invoke onto the UI thread, should be in an async fashion
            Task.Run(() =>
            {
                //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
                if (!callback.IsDisposed)
                {
                    using (callback)
                    {
                        //We'll allow the expired certificate from badssl.com
                        if (requestUrl.ToLower().Contains("https://expired.badssl.com/"))
                        {
                            callback.Continue(true);
                        }
                        else
                        {
                            callback.Continue(false);
                        }
                    }
                }
            });

            return true;

            //Example #2
            //Execute the callback and return true to immediately allow the invalid certificate
            //callback.Continue(true); //Callback will Dispose it's self once exeucted
            //return true;

            //Example #3
            //Return false for the default behaviour (cancel request immediately)
            //callback.Dispose(); //Dispose of callback
            //return false;
        }

        protected override void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            //Example #1
            //Spawn a Task to execute our callback and return true;
            //Typical usage would see you invoke onto the UI thread to open a username/password dialog
            //Then execute the callback with the response username/password
            //You can cast the IWebBrowser param to ChromiumWebBrowser to easily access
            //control, from there you can invoke onto the UI thread, should be in an async fashion
            //Load https://httpbin.org/basic-auth/cefsharp/passwd in the browser to test
            Task.Run(() =>
            {
                using (callback)
                {
                    if (originUrl.Contains("https://httpbin.org/basic-auth/"))
                    {
                        var parts = originUrl.Split('/');
                        var username = parts[parts.Length - 2];
                        var password = parts[parts.Length - 1];
                        callback.Continue(username, password);
                    }
                }
            });

            return true;

            //Example #2
            //Return false to cancel the request
            //callback.Dispose();
            //return false;
        }

        protected override void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
            chromiumWebBrowser.Load(CefExample.RenderProcessCrashedUrl);
        }

        protected override bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
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
            if (request.Url.ToLower().StartsWith("https://cefsharp.example")
                || request.Url.ToLower().StartsWith(CefSharpSchemeHandlerFactory.SchemeName)
                || request.Url.ToLower().StartsWith("mailto:")
                || request.Url.ToLower().StartsWith("https://googlechrome.github.io/samples/service-worker/"))
            {
                return new ExampleResourceRequestHandler();
            }

            return null;
        }
    }
}

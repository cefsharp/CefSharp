// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Text;
using CefSharp.Example.Filters;
using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
    /// <summary>
    /// ExampleResourceRequestHandler demonstrates some of the features you can perform
    /// using a <see cref="ResourceRequestHandler"/>
    /// </summary>
    /// <remarks>
    /// A <see cref="ResourceRequestHandler"/> represents a single resource request
    /// </remarks>
    public class ExampleResourceRequestHandler : ResourceRequestHandler
    {
        private MemoryStream memoryStream;

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                //If we're unable to parse the Uri then cancel the request
                // avoid throwing any exceptions here as we're being called by unmanaged code
                return CefReturnValue.Cancel;
            }

            //Example of how to set Referer
            // Same should work when setting any header

            // For this example only set Referer when using our custom scheme
            if (url.Scheme == CefSharpSchemeHandlerFactory.SchemeName)
            {
                //Referrer is now set using it's own method (was previously set in headers before)
                request.SetReferrer("http://google.com", ReferrerPolicy.Default);
            }

            //Example of setting User-Agent in every request.
            //var headers = request.Headers;

            //var userAgent = headers["User-Agent"];
            //headers["User-Agent"] = userAgent + " CefSharp";

            //request.Headers = headers;

            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    if (request.Method == "POST")
                    {
                        using (var postData = request.PostData)
                        {
                            if (postData != null)
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
            }

            return CefReturnValue.Continue;
        }

        protected override void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //    newUrl = "https://github.com";
            //}
        }

        protected override bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return request.Url.StartsWith("mailto");
        }

        protected override bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            //NOTE: You cannot modify the response, only the request
            // You can now access the headers
            //var headers = response.Headers;

            return false;
        }

        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var url = new Uri(request.Url);
            if (url.Scheme == CefSharpSchemeHandlerFactory.SchemeName)
            {
                if (request.Url.Equals(CefExample.ResponseFilterTestUrl, StringComparison.OrdinalIgnoreCase))
                {
                    return new FindReplaceResponseFilter("REPLACE_THIS_STRING", "This is the replaced string!");
                }

                if (request.Url.Equals("custom://cefsharp/assets/js/jquery.js", StringComparison.OrdinalIgnoreCase))
                {
                    return new AppendResponseFilter(System.Environment.NewLine + "//CefSharp Appended this comment.");
                }

                //Only called for our customScheme
                memoryStream = new MemoryStream();
                return new StreamResponseFilter(memoryStream);
            }

            //return new PassThruResponseFilter();
            return null;
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            var url = new Uri(request.Url);
            if (url.Scheme == CefSharpSchemeHandlerFactory.SchemeName && memoryStream != null)
            {
                //TODO: Do something with the data here
                var data = memoryStream.ToArray();
                var dataLength = data.Length;
                //NOTE: You may need to use a different encoding depending on the request
                var dataAsUtf8String = Encoding.UTF8.GetString(data);
            }
        }
    }
}

// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.Handlers
{
    public class RequestContextHandler : CefSharp.Handler.RequestContextHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            // Return null for the default behaviour
            //return null;

            // To handle resource requests at the RequestContext level
            // Implement CefSharp.IResourceRequestHandler or inherit from CefSharp.Handler.ResourceRequestHandler
            return new ExampleResourceRequestHandler();
        }

        protected override void OnRequestContextInitialized(IRequestContext requestContext)
        {
            // You can set preferences here on your newly initialized request context.
            // Note, there is called on the CEF UI Thread, so you can directly call SetPreference

            // Use this to check that settings preferences are working in your code
            // You should see the minimum font size is now 24pt
            //string errorMessage;
            //var success = requestContext.SetPreference("webkit.webprefs.minimum_font_size", 24, out errorMessage);

            // This is the preferred place to set the proxy as it's called before the first request is made,
            // ensuring all requests go through the specified proxy
            //string errorMessage;
            //bool success = requestContext.SetProxy("http://localhost:8080", out errorMessage); 
        }
    }
}

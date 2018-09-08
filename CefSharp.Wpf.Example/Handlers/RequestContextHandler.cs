// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf.Example.Handlers
{
    public class RequestContextHandler : IRequestContextHandler
    {
        private ICookieManager customCookieManager;

        bool IRequestContextHandler.OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy)
        {
            //pluginPolicy = PluginPolicy.Disable;
            //return true;

            return false;
        }

        ICookieManager IRequestContextHandler.GetCookieManager()
        {
            if (customCookieManager == null)
            {
                //In memory cookie manager	
                //customCookieManager = new CookieManager(null, persistSessionCookies: false, callback: null);

                //Store cookies in cookies directory (user must have write permission to this folder)
                customCookieManager = new CookieManager("cookies", persistSessionCookies: false, callback: null);
            }

            return customCookieManager;

            //NOTE: DO NOT RETURN A NEW COOKIE MANAGER EVERY TIME
            //This method will be called many times, you should return the same cookie manager within the scope
            //of the RequestContext (unless you REALLY know what your doing)
            //return new CookieManager("cookies", persistSessionCookies: false, callback: null);

            //Default to using the Global cookieManager (default)
            //return null;
        }

        void IRequestContextHandler.OnRequestContextInitialized(IRequestContext requestContext)
        {
            //You can set preferences here on your newly initialized request context.
            //Note, there is called on the CEF UI Thread, so you can directly call SetPreference

            //Use this to check that settings preferences are working in your code
            //string errorMessage;
            //var success = requestContext.SetPreference("webkit.webprefs.minimum_font_size", 24, out errorMessage);

            //You can set the proxy with code similar to the code below
            //var v = new Dictionary<string, object>
            //{
            //    ["mode"] = "fixed_servers",
            //    ["server"] = "scheme://host:port"
            //};
            //string errorMessage;
            //bool success = requestContext.SetPreference("proxy", v, out errorMessage);
        }
    }
}

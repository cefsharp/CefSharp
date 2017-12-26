// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
            if(customCookieManager == null)
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
    }
}

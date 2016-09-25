// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf.Example.Handlers
{
    public class RequestContextHandler : IRequestContextHandler
    {
        bool IRequestContextHandler.OnBeforePluginLoad(string mimeType, string url, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy)
        {
            //pluginPolicy = PluginPolicy.Disable;
            //return true;

            return false;
        }

        ICookieManager IRequestContextHandler.GetCookieManager()
        {
            //Provide your own cookie manager 
            return null;
        }
    }
}

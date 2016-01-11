// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IPluginHandler
    {
        /// <summary>
        /// Called on the browser process IO thread before a plugin instance is loaded.
        /// The default plugin policy can be set at runtime using the `--plugin-policy=[allow|detect|block]` command-line flag.
        /// </summary>
        /// <param name="mimeType">is the mime type of the plugin that will be loaded</param>
        /// <param name="url">is the content URL that the plugin will load and may be empty</param>
        /// <param name="topOriginUrl">is the URL for the top-level frame that contains the plugin</param>
        /// <param name="info">includes additional information about the plugin that will be loaded</param>
        /// <param name="pluginPolicy">Modify and return true to change the policy.</param>
        /// <returns>Return false to use the recommended policy. Modify and return true to change the policy.</returns>
        bool OnBeforePluginLoad(string mimeType, string url, string topOriginUrl, WebPluginInfo pluginInfo, ref PluginPolicy pluginPolicy);
    }
}

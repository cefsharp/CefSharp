// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example.Handlers
{
    public class ExtensionHandler : IExtensionHandler
    {
        public Func<IExtension, bool, IBrowser> GetActiveBrowser;
        public Action<string> LoadExtensionPopup;

        public void Dispose()
        {
            GetActiveBrowser = null;
            LoadExtensionPopup = null;
        }

        bool IExtensionHandler.CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return false;
        }

        IBrowser IExtensionHandler.GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
            return GetActiveBrowser?.Invoke(extension, includeIncognito);
            return null;
        }

        bool IExtensionHandler.GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback)
        {
            return false;
        }

        bool IExtensionHandler.OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings)
        {
            return false;
        }

        bool IExtensionHandler.OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings)
        {
            return false;
        }

        void IExtensionHandler.OnExtensionLoaded(IExtension extension)
        {
            var manifest = extension.Manifest;
            var browserAction = manifest["browser_action"].GetDictionary();
            if (browserAction.ContainsKey("default_popup"))
            {
                var popupUrl = browserAction["default_popup"].GetString();

                popupUrl = "chrome-extension://" + extension.Identifier + "/" + popupUrl;

                LoadExtensionPopup?.Invoke(popupUrl);
            }
        }

        void IExtensionHandler.OnExtensionLoadFailed(CefErrorCode errorCode)
        {
            
        }

        void IExtensionHandler.OnExtensionUnloaded(IExtension extension)
        {
            
        }
    }
}

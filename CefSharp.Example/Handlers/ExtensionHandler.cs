// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.Handlers
{
    public class ExtensionHandler : IExtensionHandler
    {
        bool IExtensionHandler.CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return false;
        }

        IBrowser IExtensionHandler.GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
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
            
        }

        void IExtensionHandler.OnExtensionLoadFailed(CefErrorCode errorCode)
        {
            
        }

        void IExtensionHandler.OnExtensionUnloaded(IExtension extension)
        {
            
        }
    }
}

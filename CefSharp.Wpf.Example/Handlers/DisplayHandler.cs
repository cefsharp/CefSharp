// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Structs;
using System;
using System.Collections.Generic;
using CursorType = CefSharp.Enums.CursorType;
using Size = CefSharp.Structs.Size;

namespace CefSharp.Wpf.Example.Handlers
{
    public class DisplayHandler : IDisplayHandler
    {

        void IDisplayHandler.OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs)
        {

        }

        bool IDisplayHandler.OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, Size newSize)
        {
            return false;
        }

        bool IDisplayHandler.OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            return false;
        }

        void IDisplayHandler.OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs)
        {

        }

        void IDisplayHandler.OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls)
        {

        }

        void IDisplayHandler.OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {

        }

        void IDisplayHandler.OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {

        }

        bool IDisplayHandler.OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
        {
            //text = "Sample text";
            return false;
        }

        void IDisplayHandler.OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs)
        {

        }

        bool IDisplayHandler.OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return false;
        }
    }
}

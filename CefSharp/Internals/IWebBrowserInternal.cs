// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public interface IWebBrowserInternal : IWebBrowser
    {
        void OnInitialized();

        void SetAddress(string address);
        void SetIsLoading(bool isloading);
        void SetNavState(bool canGoBack, bool canGoForward, bool canReload);
        void SetTitle(string title);
        void SetTooltipText(string tooltipText);

        void OnFrameLoadStart(string url, bool isMainFrame);
        void OnFrameLoadEnd(string url, bool isMainFrame, int httpStatusCode);
        void OnConsoleMessage(string message, string source, int line);
        void OnStatusMessage(string value);
        void OnLoadError(string url, CefErrorCode errorCode, string errorText);
    }
}

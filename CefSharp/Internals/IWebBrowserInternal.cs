// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    /// <summary>
    /// Interface implemented by UI control that contains
    /// a ManagedCefBrowserAdapter instance.
    /// </summary>
    public interface IWebBrowserInternal : IWebBrowser
    {
        void OnInitialized();

        void SetAddress(string address);
        void SetLoadingStateChange(bool canGoBack, bool canGoForward, bool isLoading);
        void SetTitle(string title);
        void SetTooltipText(string tooltipText);

        void OnFrameLoadStart(IFrame frame);
        void OnFrameLoadEnd(IFrame frame, int httpStatusCode);
        void OnConsoleMessage(string message, string source, int line);
        void OnStatusMessage(string value);
        void OnLoadError(IFrame frame, CefErrorCode errorCode, string errorText, string failedUrl);
    }
}

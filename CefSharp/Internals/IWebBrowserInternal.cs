// Copyright © 2014 The CefSharp Authors. All rights reserved.
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
        void OnAfterBrowserCreated(IBrowser browser);

        void SetAddress(AddressChangedEventArgs args);
        void SetLoadingStateChange(LoadingStateChangedEventArgs args);
        void SetTitle(TitleChangedEventArgs args);
        void SetTooltipText(string tooltipText);
        void SetCanExecuteJavascriptOnMainFrame(bool canExecute);
        void SetJavascriptMessageReceived(JavascriptMessageReceivedEventArgs args);

        void OnFrameLoadStart(FrameLoadStartEventArgs args);
        void OnFrameLoadEnd(FrameLoadEndEventArgs args);
        void OnConsoleMessage(ConsoleMessageEventArgs args);
        void OnStatusMessage(StatusMessageEventArgs args);
        void OnLoadError(LoadErrorEventArgs args);

        IBrowserAdapter BrowserAdapter { get; }
        bool HasParent { get; set; }
    }
}

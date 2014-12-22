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

        /// <summary>
        /// Called when the browser component is about to lose focus. For instance, if focus was on the last HTML element and the user pressed the TAB key. 
        /// </summary>
        /// <param name="next">will be true if the browser is giving focus to the next component and false if the browser is giving focus to the previous component.</param>
        void OnTakeFocus(bool next);

        /// <summary>
        /// Called when the browser component is requesting focus.
        /// </summary>
        /// <param name="source">Indicates where the focus request is originating from.</param>
        /// <returns>Return false to allow the focus to be set or true to cancel setting the focus.</returns>
        bool OnSetFocus(CefFocusSource source);

        /// <summary>
        /// Called when the browser component has received focus.
        /// </summary>
        void OnGotFocus();
    }
}

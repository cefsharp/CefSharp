﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Internals
{
    public interface IWebBrowserInternal : IWebBrowser
    {
        IDictionary<string, object> BoundObjects { get; }

        ILifeSpanHandler LifeSpanHandler { get; set; }
        IKeyboardHandler KeyboardHandler { get; set; }
        IJsDialogHandler JsDialogHandler { get; set; }

        void OnInitialized();

        void SetAddress(string address);
        void SetIsLoading(bool isloading);
        void SetNavState(bool canGoBack, bool canGoForward, bool canReload);
        void SetTitle(string title);
        void SetTooltipText(string tooltipText);
        void ShowDevTools();
        void CloseDevTools();

        void OnFrameLoadStart(string url);
        void OnFrameLoadEnd(string url);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(string message, string source, int line);
        void OnLoadError(string url, CefErrorCode errorCode, string errorText);
    }
}

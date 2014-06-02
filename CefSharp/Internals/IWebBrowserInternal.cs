using System.Collections.Generic;

namespace CefSharp.Internals
{
    public interface IWebBrowserInternal : IWebBrowser
    {
        IDictionary<string, object> BoundObjects { get; }

        //TODO: shouldnt this be part of IWebBrowser
        ILifeSpanHandler LifeSpanHandler { get; set; }
        IKeyboardHandler KeyboardHandler { get; set; }
        IJsDialogHandler JsDialogHandler { get; set; }
        IDialogHandler DialogHandler { get; set; }
        //end

        void OnInitialized();

        void SetAddress(string address);
        void SetIsLoading(bool isloading);
        void SetNavState(bool canGoBack, bool canGoForward, bool canReload);
        void SetTitle(string title);
        void SetTooltipText(string tooltipText);
        void ShowDevTools();
        void CloseDevTools();

        void OnFrameLoadStart(string url, bool isMainFrame);
        void OnFrameLoadEnd(string url, bool isMainFrame, int httpStatusCode);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(string message, string source, int line);
        void OnLoadError(string url, CefErrorCode errorCode, string errorText);
    }
}

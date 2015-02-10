#include "Stdafx.h"
#pragma once

#include "BrowserCore.h"
#include "ConsoleMessageEventArgs.h"
#include "LoadCompletedEventArgs.h"
#include "LoadStartedEventArgs.h"

using namespace System;
using namespace System::ComponentModel;

namespace CefSharp
{
    interface class ILifeSpanHandler;
    interface class ILoadHandler;
    interface class IRequestHandler;
    interface class IMenuHandler;
    interface class IKeyboardHandler;
    interface class IJsDialogHandler;

    public interface class IWebBrowser : IDisposable, INotifyPropertyChanged
    {
    public:
        event ConsoleMessageEventHandler^ ConsoleMessage;

        property bool IsBrowserInitialized { bool get(); }
        property bool IsLoading { bool get(); }
        property bool CanGoBack { bool get(); }
        property bool CanGoForward { bool get(); }

        property int ContentsWidth;
        property int ContentsHeight;

        property String^ Address;
        property String^ Title;
        property String^ TooltipText;

        property ILifeSpanHandler^ LifeSpanHandler;
        property ILoadHandler^ LoadHandler;
        property IRequestHandler^ RequestHandler;
        property IMenuHandler^ MenuHandler;
        property IKeyboardHandler^ KeyboardHandler;
        property IJsDialogHandler^ JsDialogHandler;

        void OnInitialized();

        void Load(String^ url);
        void LoadHtml(String^ html);
        void Stop();
        void Back();
        void Forward();
        void Reload();
        void Reload(bool ignoreCache);
        void ClearHistory();
        void ShowDevTools();
        void CloseDevTools();

        void Undo();
        void Redo();
        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void SelectAll();
        void Print();

        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void OnFrameLoadStart(String^ url, bool isMainFrame);
        void OnFrameLoadEnd(String^ url, bool isMainFrame);
        void OnTakeFocus(bool next);
        void OnConsoleMessage(String^ message, String^ source, int line);

        void RegisterJsObject(String^ name, Object^ objectToBind);
        IDictionary<String^, Object^>^ GetBoundObjects();
    };
}
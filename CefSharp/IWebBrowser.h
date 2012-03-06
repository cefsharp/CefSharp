#include "stdafx.h"
#pragma once

#include "ConsoleMessageEventArgs.h"

using namespace System;
using namespace System::ComponentModel;

namespace CefSharp
{
    interface class IBeforePopup;
    interface class IBeforeResourceLoad;
    interface class IBeforeMenu;
    interface class IAfterResponse;

    public interface class IWebBrowser : INotifyPropertyChanged
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

        property IBeforePopup^ BeforePopupHandler;
        property IBeforeResourceLoad^ BeforeResourceLoadHandler;
        property IBeforeMenu^ BeforeMenuHandler;
        property IAfterResponse^ AfterResponseHandler;

        void OnInitialized();

        void Load(String^ url);
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

        void OnFrameLoadStart();
        void OnFrameLoadEnd();
        void OnTakeFocus(bool next);
        void OnConsoleMessage(String^ message, String^ source, int line);
    };
}
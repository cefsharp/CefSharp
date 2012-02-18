#include "stdafx.h"
#pragma once

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
        property bool IsLoading { bool get(); }
        property bool CanGoBack { bool get(); }
        property bool CanGoForward { bool get(); }

        property String^ Address;
        property String^ Title;
        property String^ Tooltip;

        property IBeforePopup^ BeforePopupHandler;
        property IBeforeResourceLoad^ BeforeResourceLoadHandler;
        property IBeforeMenu^ BeforeMenuHandler;
        property IAfterResponse^ AfterResponseHandler;

        void OnInitialized();
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        void RaiseConsoleMessage(String^ message, String^ source, int line);
    };
}
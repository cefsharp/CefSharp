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

    public interface class ICefWebBrowser : INotifyPropertyChanged
    {
    public:
        property IBeforePopup^ BeforePopupHandler;
        property IBeforeResourceLoad^ BeforeResourceLoadHandler;
        property IBeforeMenu^ BeforeMenuHandler;
        property IAfterResponse^ AfterResponseHandler;

        void OnInitialized();

        void SetTitle(String^ title);
        void SetToolTip(String^ text);
        void SetAddress(String^ address);
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        void SetJsResult(String^ result);
        void SetJsError();

        void RaiseConsoleMessage(String^ message, String^ source, int line);
    };
}
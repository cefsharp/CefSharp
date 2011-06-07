#include "stdafx.h"
#pragma once

using namespace System;
using namespace System::ComponentModel;

namespace CefSharp 
{
    interface class IBeforeCreated;
    interface class IBeforeResourceLoad;

    public interface class ICefWebBrowser : INotifyPropertyChanged
    {
    public:
        property IBeforeCreated^ BeforeCreatedHandler;
        property IBeforeResourceLoad^ BeforeResourceLoadHandler;

        void OnInitialized();

        void SetTitle(String^ title);
        void SetAddress(String^ address);
        void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);
        
        void AddFrame(CefRefPtr<CefFrame> frame);
        void FrameLoadComplete(CefRefPtr<CefFrame> frame);

        void SetJsResult(String^ result);
        void SetJsError();

        void RaiseConsoleMessage(String^ message, String^ source, int line);
    };
}
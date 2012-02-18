#include "stdafx.h"

#include "BrowserCore.h"

namespace CefSharp
{
    void BrowserCore::SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
    {
        if(isLoading != _isLoading) 
        {
            _isLoading = isLoading;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"IsLoading"));
        }

        if(canGoBack != _canGoBack) 
        {
            _canGoBack = canGoBack;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoBack"));
        }

        if(canGoForward != _canGoForward)
        {
            _canGoForward = canGoForward;
            PropertyChanged(this, gcnew PropertyChangedEventArgs(L"CanGoForward"));
        }
    }

    void BrowserCore::AddFrame(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->AddCount();
    }

    void BrowserCore::FrameLoadComplete(CefRefPtr<CefFrame> frame)
    {
        _loadCompleted->Signal();
    }
}
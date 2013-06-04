#include "stdafx.h"

#include "BrowserCore.h"

namespace CefSharp
{
    void BrowserCore::CheckBrowserInitialization()
    {
        if (!_isBrowserInitialized)
        {
            throw gcnew InvalidOperationException("Browser is not initialized");
        }
    }

    void BrowserCore::RegisterJsObject(String^ name, Object^ objectToBind)
    {
        _boundObjects[name] = objectToBind;
    }

    IDictionary<String^, Object^>^ BrowserCore::GetBoundObjects()
    {
        return _boundObjects;
    }

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

    void BrowserCore::OnInitialized()
    {
        _isBrowserInitialized = true;
        PropertyChanged(this, gcnew PropertyChangedEventArgs(L"IsBrowserInitialized"));
    }

    void BrowserCore::OnLoad()
    {
        _loadCompleted->Reset();
    }

    void BrowserCore::OnFrameLoadStart()
    {
        _loadCompleted->AddCount();
    }

    void BrowserCore::OnFrameLoadEnd()
    {
        _loadCompleted->Signal();
    }
}
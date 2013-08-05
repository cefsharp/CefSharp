// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
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
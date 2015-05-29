// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefBrowserHostWrapper.h"

void CefBrowserHostWrapper::StartDownload(String^ url)
{
    _browserHost->StartDownload(StringUtils::ToNative(url));
}

void CefBrowserHostWrapper::Print()
{
    _browserHost->Print();
}

double CefBrowserHostWrapper::GetZoomLevelAsync()
{
    return 0;
}

IntPtr CefBrowserHostWrapper::GetWindowHandle()
{
    return IntPtr(_browserHost->GetWindowHandle());
}

void CefBrowserHostWrapper::CloseBrowser(bool forceClose)
{
    _browserHost->CloseBrowser(forceClose);
}
        
void CefBrowserHostWrapper::ShowDevTools()
{
}

void CefBrowserHostWrapper::CloseDevTools()
{
}

void CefBrowserHostWrapper::AddWordToDictionary(String^ word)
{
    _browserHost->AddWordToDictionary(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::ReplaceMisspelling(String^ word)
{
    _browserHost->ReplaceMisspelling(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext)
{
}

void CefBrowserHostWrapper::StopFinding(bool clearSelection)
{
}

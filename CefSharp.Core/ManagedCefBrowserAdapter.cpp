// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Cef.h"

double ManagedCefBrowserAdapter::GetZoomLevel()
{
    CefTaskScheduler::EnsureOn(TID_UI, "ManagedCefBrowserAdapter::GetZoomLevel");

    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        auto host = browser->GetHost();
        return host->GetZoomLevel();
    }
    return 0.0;
}

Task<double>^ ManagedCefBrowserAdapter::GetZoomLevelAsync()
{
    if (CefCurrentlyOn(TID_UI))
    {
        TaskCompletionSource<double>^ taskSource = gcnew TaskCompletionSource<double>();
        taskSource->SetResult(GetZoomLevel());
        return taskSource->Task;
    }
    return Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &ManagedCefBrowserAdapter::GetZoomLevel));
}

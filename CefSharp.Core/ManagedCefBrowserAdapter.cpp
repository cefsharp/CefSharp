#include "Stdafx.h"
#include "Cef.h"

double ManagedCefBrowserAdapter::GetZoomLevel()
{
    auto browser = _clientAdapter->GetCefBrowser();

    if (browser != nullptr)
    {
        auto host = browser->GetHost();
        if (CefCurrentlyOn(TID_UI))
        {
            return host->GetZoomLevel();
        }
        else
        {
            // TODO: Add an async version of GetZoomLevel at some point.
            // NOTE: Use of ManualResetEvent is required here in order
            // for simple marshaling of some kind of synchronization primitive
            // to the callback.
            Task<double>^ task = Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &ManagedCefBrowserAdapter::GetZoomLevel));
            task->Wait();
            return task->Result;
        }
    }
    return 0.0;
}

Task<double>^ ManagedCefBrowserAdapter::GetZoomLevelAsync()
{
    return Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &ManagedCefBrowserAdapter::GetZoomLevel));
}

#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public interface class IAfterLoadError
    {
    public:
        void HandleLoadError(IWebBrowser^ browser, int errorCode, String^ failedUrl, String^% errorText);
    };
}
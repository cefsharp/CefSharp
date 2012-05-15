#include "stdafx.h"
#pragma once

using namespace System;
using namespace System::Net;

namespace CefSharp
{
    public interface class IAfterResponse
    {
    public:
        void HandleResponse(IWebBrowser^ browser, String^ url, int status, String^ statusText, String^ mimeType, WebHeaderCollection^ headers);
        void HandleError(IWebBrowser^ browser, String^ url, int errorCode, String^% errorText);
    };
}
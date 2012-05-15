#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public ref struct Header
    {
    public:
        String^ name;
        String^ value;
    };

    public interface class IAfterResponse
    {
    public:
        void HandleResponse(IWebBrowser^ browser, String^ url, int status, String^ statusText, String^ mimeType, IList<Header^>^ headers);
        void HandleError(IWebBrowser^ browser, String^ url, int errorCode, String^% errorText);
    };
}
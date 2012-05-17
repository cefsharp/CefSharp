#include "stdafx.h"
#pragma once

#include "RequestResponse.h"

using namespace System;
using namespace System::Net;

namespace CefSharp
{
    public enum class NavigationType
    {
        LinkClicked = NAVTYPE_LINKCLICKED,
        FormSubmitted = NAVTYPE_FORMSUBMITTED,
        BackForward = NAVTYPE_BACKFORWARD,
        Reload = NAVTYPE_RELOAD,
        FormResubmitted = NAVTYPE_FORMRESUBMITTED,
        Other = NAVTYPE_OTHER,
        LinkDropped = NAVTYPE_LINKDROPPED,
    };

    public interface class IRequestHandler
    {
    public:
        bool OnBeforeBrowse(IWebBrowser^ browser, IRequest^ request, NavigationType naigationvType, bool isRedirect);
        bool OnBeforeResourceLoad(IWebBrowser^ browser, IRequestResponse^ requestResponse);
        void OnResourceResponse(IWebBrowser^ browser, String^ url, int status, String^ statusText, String^ mimeType, WebHeaderCollection^ headers);
    };
}
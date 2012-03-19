#include "stdafx.h"
#pragma once

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

    public interface class IBeforeBrowse
    {
    public:
        // called before page navigation.
        // allows the Request to be cancelled by returning true
        bool HandleBeforeBrowse(IWebBrowser^ browserControl, IRequest^ request, NavigationType naigationvType, bool isRedirect);
    };
}
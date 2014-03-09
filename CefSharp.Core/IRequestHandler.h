// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IRequestResponse.h"

using namespace System;
using namespace System::Net;
using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    public enum class NavigationType
    {
        LinkClicked = NAVIGATION_LINK_CLICKED,
        FormSubmitted = NAVIGATION_FORM_SUBMITTED,
        BackForward = NAVIGATION_BACK_FORWARD,
        Reload = NAVIGATION_RELOAD,
        FormResubmitted = NAVIGATION_FORM_RESUBMITTED,
        Other = NAVIGATION_OTHER
    };

    public interface class IRequestHandler
    {
        bool OnBeforeBrowse(IWebBrowser^ browser, IRequest^ request, NavigationType navigationType, bool isRedirect);
        bool OnBeforeResourceLoad(IWebBrowser^ browser, IRequestResponse^ requestResponse);
        void OnResourceResponse(IWebBrowser^ browser, String^ url, int status, String^ statusText, String^ mimeType, WebHeaderCollection^ headers);
        bool GetDownloadHandler(IWebBrowser^ browser, [Out] IDownloadHandler ^% handler);
        bool GetAuthCredentials(IWebBrowser^ browser, bool isProxy, String^ host ,int port, String^ realm, String^ scheme, String^% username, String^% password);
   };
}

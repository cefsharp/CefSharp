// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals\MCefRefPtr.h"
#include "RequestContextHandler.h"
#include "include\cef_request_context.h"

using namespace CefSharp;

namespace CefSharp
{
    public ref class RequestContext
    {
        MCefRefPtr<CefRequestContext> _requestContext;

    public:
        RequestContext(String^ cookiePath, bool persistSessionCookies)
        {
            auto cookieManager = CefCookieManager::CreateManager(StringUtils::ToNative(cookiePath), persistSessionCookies);
            CefRefPtr<CefRequestContextHandler> requestContextHandler = new RequestContextHandler(cookieManager);
            _requestContext = CefRequestContext::CreateContext(requestContextHandler);
        }

        !RequestContext()
        {
            _requestContext = NULL;
        }

        ~RequestContext()
        {
            this->!RequestContext();
        }

        operator CefRefPtr<CefRequestContext>()
        {
            return _requestContext.get();
        }
    };
}
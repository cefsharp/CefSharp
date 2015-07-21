
// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_request_context_handler.h"
#include "include\cef_cookie.h"

namespace CefSharp
{
    private class RequestContextHandler : public CefRequestContextHandler
    {
    private:
        CefRefPtr<CefCookieManager> _cookieManager;

    public:
        RequestContextHandler(CefRefPtr<CefCookieManager> cookieManager) : _cookieManager(cookieManager)
        {
        }
        
        ~RequestContextHandler()
        {
            _cookieManager = NULL;
        };

        CefRefPtr<CefCookieManager> GetCookieManager() OVERRIDE
        {
            return _cookieManager;
        }

        IMPLEMENT_REFCOUNTING(RequestContextHandler);
    };
}
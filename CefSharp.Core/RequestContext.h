// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"
#include "CookieManager.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class RequestContext
    {
    private:
        bool _isFinalized;
        CookieManager^ _cookieManager;
        MCefRefPtr<CefRequestContext> _requestContext;

    public:
        RequestContext(CookieManager^ cookieManager) :
            _cookieManager(cookieManager),
            _requestContext(CefRequestContext::CreateContext(
                cookieManager->GetHandler())),
            _isFinalized(false) { }

        !RequestContext()
        {
            _requestContext = NULL;
            _cookieManager = nullptr;
            _isFinalized = true;
        }

        ~RequestContext()
        {
            if (!_isFinalized) this->!RequestContext();
        }

        operator CefRefPtr<CefRequestContext>()
        {
            return *new CefRefPtr<CefRequestContext>(_requestContext.get());
        }
    };
}
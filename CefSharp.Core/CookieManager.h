
// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"
#include <include/cef_request_context_handler.h>
#include "RequestContextHandler.h"
#include "Internals/MCefRefPtr.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CookieManager
    {
    private:
        MCefRefPtr<CefRequestContextHandler> _handler;

    internal:
        CefRefPtr<CefRequestContextHandler> GetHandler()
        {
            return *new CefRefPtr<CefRequestContextHandler>(_handler.get());
        }

    public:
        CookieManager(String^ cookiePath, bool persistSessionCookies) :
            _handler((CefRequestContextHandler*)new RequestContextHandler(
                    CefCookieManager::CreateManager(
                        StringUtils::ToNative(cookiePath),
                        persistSessionCookies))) { }

        !CookieManager()
        {
            this->_handler = NULL;
        }

        ~CookieManager()
        {
            this->!CookieManager();
        }

    };
}
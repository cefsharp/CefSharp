// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_cookie.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefSetCookieCallbackAdapter : public CefSetCookieCallback
        {
        private:
            gcroot<ISetCookieCallback^> _handler;

        public:
            CefSetCookieCallbackAdapter(ISetCookieCallback^ handler)
            {
                _handler = handler;
            }

            ~CefSetCookieCallbackAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            void OnComplete(bool success) OVERRIDE
            {
                _handler->OnComplete(success);
            }

            IMPLEMENT_REFCOUNTING(CefSetCookieCallbackAdapter);
        };
    }
}




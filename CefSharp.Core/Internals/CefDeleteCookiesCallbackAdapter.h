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
        private class CefDeleteCookiesCallbackAdapter : public CefDeleteCookiesCallback
        {
        private:
            gcroot<IDeleteCookiesCallback^> _handler;

        public:
            CefDeleteCookiesCallbackAdapter(IDeleteCookiesCallback^ handler)
            {
                _handler = handler;
            }

            ~CefDeleteCookiesCallbackAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            void OnComplete(int numDeleted) OVERRIDE
            {
                _handler->OnComplete(numDeleted);
            }

            IMPLEMENT_REFCOUNTING(CefDeleteCookiesCallbackAdapter);
        };
    }
}


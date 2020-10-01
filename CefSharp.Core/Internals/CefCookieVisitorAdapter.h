// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "TypeConversion.h"
#include "include/cef_cookie.h"


namespace CefSharp
{
    namespace Internals
    {
        private class CefCookieVisitorAdapter : public CefCookieVisitor
        {
        private:
            gcroot<ICookieVisitor^> _visitor;

        public:
            CefCookieVisitorAdapter(ICookieVisitor^ visitor) :
                _visitor(visitor)
            {
            }

            ~CefCookieVisitorAdapter()
            {
                delete _visitor;
                _visitor = nullptr;
            }

            virtual bool Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie) OVERRIDE
            {
                auto cookie = TypeConversion::FromNative(cefCookie);

                return _visitor->Visit(cookie, count, total, deleteCookie);
            }

            IMPLEMENT_REFCOUNTING(CefCookieVisitorAdapter);
        };
    }
}

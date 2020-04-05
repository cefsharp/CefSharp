// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "TypeConversion.h"
#include "include/cef_cookie.h"


namespace CefSharp
{
    private class CefCookieVisitorAdaptor : public CefCookieVisitor
    {
    private:
        gcroot<ICookieVisitor^> _visitor;

    public:
        CefCookieVisitorAdaptor(ICookieVisitor^ visitor) :
            _visitor(visitor)
        {
        }

        ~CefCookieVisitorAdaptor()
        {
            delete _visitor;
            _visitor = nullptr;
        }

        virtual bool Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie) OVERRIDE
        {
            auto cookie = TypeConversion::FromNative(cefCookie);

            return _visitor->Visit(cookie, count, total, deleteCookie);
        }

        IMPLEMENT_REFCOUNTING(CefCookieVisitorAdaptor);
    };
}

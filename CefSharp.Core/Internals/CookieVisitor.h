// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_cookie.h"
#include "ICookieVisitor.h"

namespace CefSharp
{
    private class CookieVisitor : public CefCookieVisitor
    {
    private:
        gcroot<ICookieVisitor^> _visitor;

    public:
        CookieVisitor(ICookieVisitor^ visitor)
        {
            _visitor = visitor;
        }

        virtual bool Visit(const CefCookie& cookie, int count, int total, bool& deleteCookie) OVERRIDE;

        IMPLEMENT_REFCOUNTING(CookieVisitor);
    };
}
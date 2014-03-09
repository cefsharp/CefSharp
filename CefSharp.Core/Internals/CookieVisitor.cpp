// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieVisitor.h"

using namespace System::Net;

namespace CefSharp
{
    bool CookieVisitor::Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie)
    {
        Cookie^ cookie = gcnew Cookie();
        cookie->Name = StringUtils::ToClr(cefCookie.name);
        cookie->Value = StringUtils::ToClr(cefCookie.value);
        cookie->Domain = StringUtils::ToClr(cefCookie.domain);
        cookie->Path = StringUtils::ToClr(cefCookie.path);
        cookie->Secure = cefCookie.secure;
        cookie->HttpOnly = cefCookie.httponly;

        if(cefCookie.has_expires)
        {
            cookie->Expires = DateTime(cefCookie.expires.year,
                cefCookie.expires.month, cefCookie.expires.day_of_month);
        }

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}
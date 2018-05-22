// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieVisitor.h"

namespace CefSharp
{
    bool CookieVisitor::Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie)
    {
        auto cookie = gcnew Cookie();
        String^ cookieName = StringUtils::ToClr(cefCookie.name);

        if (!String::IsNullOrEmpty(cookieName))
        {
            cookie->Name = StringUtils::ToClr(cefCookie.name);
            cookie->Value = StringUtils::ToClr(cefCookie.value);
            cookie->Domain = StringUtils::ToClr(cefCookie.domain);
            cookie->Path = StringUtils::ToClr(cefCookie.path);
            cookie->Secure = cefCookie.secure == 1;
            cookie->HttpOnly = cefCookie.httponly == 1;

            if (cefCookie.has_expires)
            {
                auto expires = cefCookie.expires;
                cookie->Expires = DateTimeUtils::FromCefTime(expires.year,
                    expires.month,
                    expires.day_of_month,
                    expires.hour,
                    expires.minute,
                    expires.second,
                    expires.millisecond);
            }


            auto creation = cefCookie.creation;
            cookie->Creation = DateTimeUtils::FromCefTime(creation.year,
                creation.month,
                creation.day_of_month,
                creation.hour,
                creation.minute,
                creation.second,
                creation.millisecond);

            auto lastAccess = cefCookie.last_access;
            cookie->LastAccess = DateTimeUtils::FromCefTime(lastAccess.year,
                lastAccess.month,
                lastAccess.day_of_month,
                lastAccess.hour,
                lastAccess.minute,
                lastAccess.second,
                lastAccess.millisecond);
        }

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}

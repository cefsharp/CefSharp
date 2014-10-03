// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
                cookie->Expires = DateTime(
                    cefCookie.expires.year,
                    cefCookie.expires.month,
                    cefCookie.expires.day_of_month,
                    cefCookie.expires.hour,
                    cefCookie.expires.minute,
                    cefCookie.expires.second,
                    cefCookie.expires.millisecond
                );
            }
        }

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}

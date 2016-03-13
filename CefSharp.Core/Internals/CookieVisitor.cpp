// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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

            try 
            {
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
            catch (Exception^ ex)
            {
                cookie->Expires = DateTime::MinValue;
            }

            //TODO: There is a method in TypeUtils that's in BrowserSubProcess that convers CefTime, need to make it accessible.
            try 
            {
                cookie->Creation = DateTime(
                    cefCookie.creation.year,
                    cefCookie.creation.month,
                    cefCookie.creation.day_of_month,
                    cefCookie.creation.hour,
                    cefCookie.creation.minute,
                    cefCookie.creation.second,
                    cefCookie.creation.millisecond
                    );
            }
            catch (Exception^ ex)
            {
                cookie->Creation = DateTime::MinValue;
            }

            try
            {
                cookie->LastAccess = DateTime(
                    cefCookie.last_access.year,
                    cefCookie.last_access.month,
                    cefCookie.last_access.day_of_month,
                    cefCookie.last_access.hour,
                    cefCookie.last_access.minute,
                    cefCookie.last_access.second,
                    cefCookie.last_access.millisecond
                    );
            }
            catch (Exception^ ex)
            {
                cookie->LastAccess = DateTime::MinValue;
            }
        }

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}

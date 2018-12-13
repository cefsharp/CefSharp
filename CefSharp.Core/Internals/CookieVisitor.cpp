// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CookieVisitor.h"
#include "TypeConversion.h"

namespace CefSharp
{
    bool CookieVisitor::Visit(const CefCookie& cefCookie, int count, int total, bool& deleteCookie)
    {
        auto cookie = TypeConversion::FromNative(cefCookie);

        return _visitor->Visit(cookie, count, total, deleteCookie);
    }
}

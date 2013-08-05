// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Net;

namespace CefSharp
{
    public interface class ICookieVisitor
    {
        bool Visit(Cookie^ cookie, int count, int total, bool%  deleteCookie);
    };
}
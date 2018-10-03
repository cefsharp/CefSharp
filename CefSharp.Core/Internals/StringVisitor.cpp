// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "StringVisitor.h"

namespace CefSharp
{
    void StringVisitor::Visit(const CefString& string)
    {
        _visitor->Visit(StringUtils::ToClr(string));
    }
}

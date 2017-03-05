// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_string_visitor.h"

namespace CefSharp
{
    private class StringVisitor : public CefStringVisitor
    {
    private:
        gcroot<IStringVisitor^> _visitor;

    public:
        StringVisitor(IStringVisitor^ visitor) :
            _visitor(visitor)
        {
        }

        ~StringVisitor()
        {
            delete _visitor;
            _visitor = nullptr;
        }

        virtual void Visit(const CefString& string) OVERRIDE;

        IMPLEMENT_REFCOUNTING(StringVisitor);
    };
}
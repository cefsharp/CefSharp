// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_string_visitor.h"

namespace CefSharp
{
    private class CefStringVisitorAdaptor : public CefStringVisitor
    {
    private:
        gcroot<IStringVisitor^> _visitor;

    public:
        CefStringVisitorAdaptor(IStringVisitor^ visitor) :
            _visitor(visitor)
        {
        }

        ~CefStringVisitorAdaptor()
        {
            delete _visitor;
            _visitor = nullptr;
        }

        virtual void Visit(const CefString& string) OVERRIDE
        {
            _visitor->Visit(StringUtils::ToClr(string));
        }

        IMPLEMENT_REFCOUNTING(CefStringVisitorAdaptor);
    };
}

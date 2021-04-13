// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_string_visitor.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefStringVisitorAdapter : public CefStringVisitor
        {
        private:
            gcroot<IStringVisitor^> _visitor;

        public:
            CefStringVisitorAdapter(IStringVisitor^ visitor) :
                _visitor(visitor)
            {
            }

            ~CefStringVisitorAdapter()
            {
                delete _visitor;
                _visitor = nullptr;
            }

            virtual void Visit(const CefString& string) OVERRIDE
            {
                _visitor->Visit(StringUtils::ToClr(string));
            }

            IMPLEMENT_REFCOUNTING(CefStringVisitorAdapter);
        };
    }
}

// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <msclr\marshal_cppstd.h>
#include <stdlib.h>
#include <string.h>
#include "include/cef_string_visitor.h"

using namespace msclr::interop;

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

            virtual void Visit(const CefString& string) override
            {
                // New Mojo IPC implementation uses shared memory rather than a string copy
                // Attempt to workaround the access violation that's been reported in a few isolated cases
                // by copying the string rather than using directly.
                // https://github.com/chromiumembedded/cef/commit/ebee84755ed14e71388b343231d3a419f1c5c1fd
                std::wstring str = string.ToWString();

                auto clrString = marshal_as<String^>(str);

                _visitor->Visit(clrString);
            }

            IMPLEMENT_REFCOUNTINGM(CefStringVisitorAdapter);
        };
    }
}

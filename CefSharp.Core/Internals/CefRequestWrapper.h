// Copyright � 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefRequestWrapper : public IRequest
        {
            MCefRefPtr<CefRequest> _wrappedRequest;
        internal:
            CefRequestWrapper(CefRefPtr<CefRequest> cefRequest) : _wrappedRequest(cefRequest) {}

        public:
            virtual property String^ Url { String^ get(); void set(String^ url); }
            virtual property String^ Method { String^ get(); }
            virtual property String^ Body { String^ get(); }
            virtual property IDictionary<String^, String^>^ Headers { IDictionary<String^, String^>^  get(); void set(IDictionary<String^, String^>^ url); }

        };
    }
}

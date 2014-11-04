// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Specialized;
using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefRequestWrapper : public IRequest
        {
            MCefRefPtr<CefRequest> _wrappedRequest;
        internal:
            CefRequestWrapper(CefRefPtr<CefRequest> cefRequest) : 
                _wrappedRequest(cefRequest) 
            {
            }

            ~CefRequestWrapper()
            {
                _wrappedRequest = nullptr;
            }

        public:
            virtual property String^ Url { String^ get(); void set(String^ url); }
            virtual property String^ Method { String^ get(); }
            virtual property String^ Body { String^ get(); }
            virtual property NameValueCollection^ Headers { NameValueCollection^ get(); void set(NameValueCollection^ url); }
            virtual property TransitionType TransitionType { CefSharp::TransitionType get(); }
        };
    }
}

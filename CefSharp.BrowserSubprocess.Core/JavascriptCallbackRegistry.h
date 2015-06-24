// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackWrapper.h"
#include "JavascriptCallbackHolder.h"

using namespace System::Collections::Concurrent;

namespace CefSharp
{
    namespace Internals
    {
        class JavascriptCallbackRegistry : public virtual CefBase
        {
        private:
            int _browserId;
            int64 _lastId;
            std::map<int64, CefRefPtr<JavascriptCallbackHolder>> _callbacks;
        public:
            JavascriptCallbackRegistry(int browserId) : _browserId(browserId), _lastId(0L)
            {
                
            }

            JavascriptCallback^ Register(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value);
            bool Execute(int64 id, const CefV8ValueList& arguments, CefRefPtr<CefV8Value> &result, CefRefPtr<CefV8Exception> &exception);
            void Deregister(int64 id);

            IMPLEMENT_REFCOUNTING(JavascriptCallbackRegistry);
        };
    }
}


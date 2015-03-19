// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackWrapper.h"

using namespace System::Collections::Concurrent;

namespace CefSharp
{
    namespace Internals
    {
        private ref class JavascriptCallbackRegistry
        {
        private:
            int _browserId;
            Int64 _lastId;
            ConcurrentDictionary<Int64, JavascriptCallbackWrapper^>^ _callbacks;
        public:
            JavascriptCallbackRegistry(int browserId) : _browserId(browserId)
            {
                _callbacks = gcnew ConcurrentDictionary<Int64, JavascriptCallbackWrapper^>();
            }
            JavascriptCallback^ Register(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value);
            JavascriptResponse^ Execute(Int64 id, array<Object^>^ params);
            void Deregister(Int64 id);
            ~JavascriptCallbackRegistry();
        };
    }
}


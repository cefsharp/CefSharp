// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackWrapper.h"
#include "ConcurrentObjectRegistry.h"

using namespace System::Collections::Concurrent;

namespace CefSharp
{
    namespace Internals
    {
        private ref class JavascriptCallbackRegistry
        {
        private:
            int _browserId;
            ConcurrentObjectRegistry<JavascriptCallbackWrapper^>^ _callbacks;

        internal:
            JavascriptCallbackWrapper^ FindWrapper(int64 id);

        public:
            JavascriptCallbackRegistry(int browserId) : _browserId(browserId)
            {
                _callbacks = gcnew ConcurrentObjectRegistry<JavascriptCallbackWrapper^>(true);
            }

            ~JavascriptCallbackRegistry()
            {
                if (_callbacks != nullptr)
                {
                    delete _callbacks;
                    _callbacks = nullptr;
                }
            }

            JavascriptCallback^ Register(const CefRefPtr<CefV8Context>& context, const CefRefPtr<CefV8Value>& value);

            void Deregister(Int64 id);
        };
    }
}


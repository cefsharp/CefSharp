// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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

        internal:
            JavascriptCallbackWrapper^ FindWrapper(int64 id);

        public:
            JavascriptCallbackRegistry(int browserId) : _browserId(browserId)
            {
                _callbacks = gcnew ConcurrentDictionary<Int64, JavascriptCallbackWrapper^>();
            }

            ~JavascriptCallbackRegistry()
            {
                if (_callbacks != nullptr)
                {
                    for each (JavascriptCallbackWrapper^ callback in _callbacks->Values)
                    {
                        delete callback;
                    }
                    _callbacks->Clear();
                    _callbacks = nullptr;
                }
            }

            JavascriptCallback^ Register(const CefRefPtr<CefV8Context>& context, const CefRefPtr<CefV8Value>& value);

            void Deregister(Int64 id);
        };
    }
}


// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Async/JavascriptAsyncMethodCallback.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Threading;
using namespace System::Collections::Generic;
using namespace CefSharp::BrowserSubprocess::Async;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        //TODO: JSB Fix naming of this class, it's pretty horrible currently
        private ref class RegisterBoundObjectRegistry
        {
        private:
            //Only access through Interlocked::Increment - used to generate unique callback Id's
            //Is static so ids are unique to this process https://github.com/cefsharp/CefSharp/issues/2792
            static int64 _lastCallback;

            initonly Dictionary<int64, JavascriptAsyncMethodCallback^>^ _methodCallbacks;

        public:
            RegisterBoundObjectRegistry()
            {
                _methodCallbacks = gcnew Dictionary<int64, JavascriptAsyncMethodCallback^>();
            }

            ~RegisterBoundObjectRegistry()
            {
                for each (JavascriptAsyncMethodCallback ^ var in _methodCallbacks->Values)
                {
                    delete var;
                }
                _methodCallbacks->Clear();
            }

            int64 SaveMethodCallback(JavascriptAsyncMethodCallback^ callback)
            {
                auto callbackId = Interlocked::Increment(_lastCallback);
                _methodCallbacks->Add(callbackId, callback);
                return callbackId;
            }

            bool TryGetAndRemoveMethodCallback(int64 id, JavascriptAsyncMethodCallback^% callback)
            {
                bool result = false;
                if (result = _methodCallbacks->TryGetValue(id, callback))
                {
                    _methodCallbacks->Remove(id);
                }
                return result;
            }
        };
    }
}

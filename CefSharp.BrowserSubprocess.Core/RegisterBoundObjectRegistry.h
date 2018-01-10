// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Async/JavascriptAsyncMethodCallback.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Threading;
using namespace System::Collections::Generic;
using namespace CefSharp::Internals::Async;

namespace CefSharp
{
    //TODO: JSB Fix naming of this class, it's pretty horrible currently
    private ref class RegisterBoundObjectRegistry
    {
    private:
        initonly Dictionary<int64, JavascriptAsyncMethodCallback^>^ _methodCallbacks;
        int64 _lastCallback;

    public:
        RegisterBoundObjectRegistry()
        {
            _methodCallbacks = gcnew Dictionary<int64, JavascriptAsyncMethodCallback^>();
        }

        ~RegisterBoundObjectRegistry()
        {
            for each(JavascriptAsyncMethodCallback^ var in _methodCallbacks->Values)
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




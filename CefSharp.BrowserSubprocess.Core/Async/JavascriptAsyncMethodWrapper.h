// Copyright � 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptAsyncMethodHandler.h"
#include "JavascriptCallbackRegistry.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncMethodWrapper
            {
            private:
                MCefRefPtr<JavascriptAsyncMethodHandler> _javascriptMethodHandler;

            public:
                JavascriptAsyncMethodWrapper(int64 ownerId, JavascriptCallbackRegistry^ callbackRegistry, CefRefPtr<CefV8Value> promiseCreator, Func<JavascriptAsyncMethodCallback^, int64>^ methodCallbackSave)
                    : _javascriptMethodHandler(new JavascriptAsyncMethodHandler(ownerId, callbackRegistry, promiseCreator, methodCallbackSave))
                {

                }

                void Bind(JavascriptMethod^ method, const CefRefPtr<CefV8Value>& value);
            };
        }
    }
}
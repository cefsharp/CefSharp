// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncMethodCallback.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            private class JavascriptAsyncMethodHandler : public virtual CefV8Handler
            {
            private:
                gcroot<JavascriptCallbackRegistry^> _callbackRegistry;
                gcroot<Func<JavascriptAsyncMethodCallback^, int64>^> _methodCallbackSave;
                int64 _objectId;

            public:
                JavascriptAsyncMethodHandler(int64 objectId, JavascriptCallbackRegistry^ callbackRegistry, Func<JavascriptAsyncMethodCallback^, int64>^ methodCallbackSave)
                    :_callbackRegistry(callbackRegistry), _objectId(objectId), _methodCallbackSave(methodCallbackSave)
                {

                }

                virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);

                ~JavascriptAsyncMethodHandler()
                {
                    // The callback registry is a shared instance among all method handlers (async & sync).
                    // It's lifecycle is managed in the JavascriptRootObjectWrapper.
                    _callbackRegistry = nullptr;
                    _methodCallbackSave = nullptr;
                }

                IMPLEMENT_REFCOUNTING(JavascriptAsyncMethodHandler);
            };
        }
    }
}

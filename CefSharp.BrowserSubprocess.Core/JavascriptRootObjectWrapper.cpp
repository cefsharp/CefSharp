// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Threading;

namespace CefSharp
{
    void JavascriptRootObjectWrapper::Bind(ICollection<JavascriptObject^>^ objects, const CefRefPtr<CefV8Value>& v8Value)
    {
        if (objects->Count > 0)
        {
            auto saveMethod = gcnew Func<JavascriptAsyncMethodCallback^, int64>(this, &JavascriptRootObjectWrapper::SaveMethodCallback);
            auto promiseCreator = v8Value->GetValue(CefAppUnmanagedWrapper::kPromiseCreatorFunction);

            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(objects))
            {
                if (obj->IsAsync)
                {
                    auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(_callbackRegistry, saveMethod);
                    wrapperObject->Bind(obj, v8Value, promiseCreator);

                    _wrappedAsyncObjects->Add(wrapperObject);
                }
                else
                {
                    auto wrapperObject = gcnew JavascriptObjectWrapper(_browserProcess);
                    wrapperObject->Bind(obj, v8Value, _callbackRegistry);

                    _wrappedObjects->Add(wrapperObject);
                }
            }
        }
    }

    JavascriptCallbackRegistry^ JavascriptRootObjectWrapper::CallbackRegistry::get()
    {
        return _callbackRegistry;
    }

    int64 JavascriptRootObjectWrapper::SaveMethodCallback(JavascriptAsyncMethodCallback^ callback)
    {
        auto callbackId = Interlocked::Increment(_lastCallback);
        _methodCallbacks->Add(callbackId, callback);
        return callbackId;
    }

    bool JavascriptRootObjectWrapper::TryGetAndRemoveMethodCallback(int64 id, JavascriptAsyncMethodCallback^% callback)
    {
        bool result = false;
        if (result = _methodCallbacks->TryGetValue(id, callback))
        {
            _methodCallbacks->Remove(id);
        }
        return result;
    }
}
// Copyright © 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Threading;

namespace CefSharp
{
    void JavascriptRootObjectWrapper::Bind(JavascriptRootObject^ rootObject, JavascriptRootObject^ asyncRootObject, const CefRefPtr<CefV8Value>& v8Value)
    {
        if (_isBound)
        {
            throw gcnew InvalidOperationException("This root object has already been bound.");
        }

        _isBound = true;

        if (rootObject != nullptr)
        {
            auto memberObjects = rootObject->MemberObjects;
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptObjectWrapper(_browserProcess);
                wrapperObject->Bind(obj, v8Value, _callbackRegistry);

                _wrappedObjects->Add(wrapperObject);
            }
        }

        if (asyncRootObject != nullptr)
        {
            auto memberObjects = asyncRootObject->MemberObjects;
            auto saveMethod = gcnew Func<JavascriptAsyncMethodCallback^, int64>(this, &JavascriptRootObjectWrapper::SaveMethodCallback);
            auto promiseCreator = v8Value->GetValue(CefAppUnmanagedWrapper::kPromiseCreatorFunction);
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(_callbackRegistry, saveMethod);
                wrapperObject->Bind(obj, v8Value, promiseCreator);

                _wrappedAsyncObjects->Add(wrapperObject);
            }
        }
    }

    JavascriptCallbackRegistry^ JavascriptRootObjectWrapper::CallbackRegistry::get()
    {
        return _callbackRegistry;
    }

    bool JavascriptRootObjectWrapper::IsBound::get()
    {
        return _isBound;
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
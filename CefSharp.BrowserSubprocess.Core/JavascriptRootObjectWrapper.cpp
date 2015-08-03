// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Threading;

namespace CefSharp
{
    void JavascriptRootObjectWrapper::Bind(const CefRefPtr<CefV8Value>& v8Value)
    {
        if (_rootObject != nullptr)
        {
            auto memberObjects = _rootObject->MemberObjects;
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptObjectWrapper(obj, _browserProcess);
                wrapperObject->Bind(v8Value, _callbackRegistry);

                _wrappedObjects->Add(wrapperObject);
            }
        }

        if (_asyncRootObject != nullptr)
        {
            auto memberObjects = _asyncRootObject->MemberObjects;
            auto saveMethod = gcnew Func<JavascriptAsyncMethodCallback^, int64>(this, &JavascriptRootObjectWrapper::SaveMethodCallback);
            auto promiseCreator = v8Value->GetValue(CefAppUnmanagedWrapper::kPromiseCreatorFunction);
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(obj, _callbackRegistry, saveMethod);
                wrapperObject->Bind(v8Value, promiseCreator);

                _wrappedAsyncObjects->Add(wrapperObject);
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
// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Threading;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        CefRefPtr<CefV8Value> JavascriptRootObjectWrapper::Bind(ICollection<JavascriptObject^>^ objects, const CefRefPtr<CefV8Value>& global)
        {
            auto result = CefV8Value::CreateArray(objects->Count);
            if (objects->Count > 0)
            {
                auto saveMethod = gcnew Func<JavascriptAsyncMethodCallback^, int64_t>(this, &JavascriptRootObjectWrapper::SaveMethodCallback);

                for each (JavascriptObject ^ obj in Enumerable::OfType<JavascriptObject^>(objects))
                {
                    const auto name = StringUtils::ToNative(obj->JavascriptName);
                    CefRefPtr<CefV8Value> v8Obj;
                    if (obj->IsAsync)
                    {
                        auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(_callbackRegistry, saveMethod);
                        v8Obj = wrapperObject->ConvertToV8Value(obj);
                        _wrappedAsyncObjects->Add(wrapperObject);
                    }
#ifndef NETCOREAPP
                    else
                    {
                        if (_browserProcess == nullptr)
                        {
                            LOG(ERROR) << StringUtils::ToNative("IBrowserProcess is null, unable to bind object " + obj->JavascriptName).ToString();

                            continue;
                        }

                        auto wrapperObject = gcnew JavascriptObjectWrapper(_browserProcess);
                        v8Obj = wrapperObject->ConvertToV8Value(obj, _callbackRegistry);
                        _wrappedObjects->Add(wrapperObject);
                    }
#endif

                    result->SetValue(name, v8Obj, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_READONLY);
                    global->SetValue(name, v8Obj, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
                }
            }

            return result;
        }

        JavascriptCallbackRegistry^ JavascriptRootObjectWrapper::CallbackRegistry::get()
        {
            return _callbackRegistry;
        }

        int64_t JavascriptRootObjectWrapper::SaveMethodCallback(JavascriptAsyncMethodCallback^ callback)
        {
            auto callbackId = Interlocked::Increment(_lastCallback);
            _methodCallbacks->Add(callbackId, callback);
            return callbackId;
        }

        bool JavascriptRootObjectWrapper::TryGetAndRemoveMethodCallback(int64_t id, JavascriptAsyncMethodCallback^% callback)
        {
            bool result = false;
            if (result = _methodCallbacks->TryGetValue(id, callback))
            {
                _methodCallbacks->Remove(id);
            }
            return result;
        }
    }
}

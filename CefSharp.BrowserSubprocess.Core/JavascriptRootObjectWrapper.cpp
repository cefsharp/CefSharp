// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "CefBrowserWrapper.h"

using namespace System::Threading;

namespace CefSharp
{
    void JavascriptRootObjectWrapper::Bind(CefBrowserWrapper^ browserWrapper, JavascriptRootObject^ rootObject, JavascriptRootObject^ asyncRootObject, const CefRefPtr<CefV8Value>& v8Value)
    {
        auto callbackRegistry = browserWrapper->CallbackRegistry;
        if (rootObject != nullptr)
        {
            auto memberObjects = rootObject->MemberObjects;
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptObjectWrapper(_browserProcess);
                wrapperObject->Bind(obj, v8Value, callbackRegistry);

                _wrappedObjects->Add(wrapperObject);
            }
        }

        if (asyncRootObject != nullptr)
        {
            auto memberObjects = asyncRootObject->MemberObjects;
            auto methodCallbackRegistry = browserWrapper->MethodCallbackRegistry;
            auto saveMethod = gcnew Func<JavascriptAsyncMethodCallback^, int64>(methodCallbackRegistry, &CefSharp::Internals::ConcurrentObjectRegistry<JavascriptAsyncMethodCallback^>::RegisterObject);
            auto promiseCreator = v8Value->GetValue(CefAppUnmanagedWrapper::kPromiseCreatorFunction);
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(callbackRegistry, saveMethod);
                wrapperObject->Bind(obj, v8Value, promiseCreator);

                _wrappedAsyncObjects->Add(wrapperObject);
            }
        }
    }
}
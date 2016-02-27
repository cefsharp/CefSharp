// Copyright � 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncObjectWrapper.h"
#include "JavascriptAsyncMethodWrapper.h"

using namespace System::Linq;

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            void JavascriptAsyncObjectWrapper::Bind(JavascriptObject^ object, const CefRefPtr<CefV8Value> &value, const CefRefPtr<CefV8Value> &promiseCreator)
            {
                //V8Value that represents this javascript object - only one per complex type, no accessor
                auto javascriptObject = CefV8Value::CreateObject(nullptr);
                auto objectName = StringUtils::ToNative(object->JavascriptName);
                value->SetValue(objectName, javascriptObject, V8_PROPERTY_ATTRIBUTE_NONE);

                for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(object->Methods))
                {
                    auto wrappedMethod = gcnew JavascriptAsyncMethodWrapper(object->Id, _callbackRegistry, promiseCreator, _methodCallbackSave);
                    wrappedMethod->Bind(method, javascriptObject);

                    _wrappedMethods->Add(wrappedMethod);
                }
            }
        }
    }
}
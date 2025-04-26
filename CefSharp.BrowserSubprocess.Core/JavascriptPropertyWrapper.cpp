// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "JavascriptPropertyWrapper.h"
#include "JavascriptObjectWrapper.h"

using namespace System;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        void JavascriptPropertyWrapper::Bind(JavascriptProperty^ javascriptProperty, const CefRefPtr<CefV8Value>& v8Value, JavascriptCallbackRegistry^ callbackRegistry)
        {
            auto propertyName = StringUtils::ToNative(javascriptProperty->JavascriptName);
            auto clrPropertyName = javascriptProperty->JavascriptName;

            if (javascriptProperty->IsComplexType)
            {
                auto javascriptObjectWrapper = gcnew JavascriptObjectWrapper(_browserProcess);
                const auto v8Obj = javascriptObjectWrapper->ConvertToV8Value(javascriptProperty->JsObject, callbackRegistry);

                auto objectName = StringUtils::ToNative(javascriptProperty->JsObject->JavascriptName);
                v8Value->SetValue(objectName, v8Obj, V8_PROPERTY_ATTRIBUTE_NONE);

                _javascriptObjectWrapper = javascriptObjectWrapper;
            }
            else
            {
                auto propertyAttribute = javascriptProperty->IsReadOnly ? V8_PROPERTY_ATTRIBUTE_READONLY : V8_PROPERTY_ATTRIBUTE_NONE;

                v8Value->SetValue(propertyName, propertyAttribute);
            }
        };
    }
}

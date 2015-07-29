﻿// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptPropertyWrapper.h"
#include "JavascriptObjectWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptPropertyWrapper::Bind(const CefRefPtr<CefV8Value>& v8Value, JavascriptCallbackRegistry^ callbackRegistry)
    {
        auto propertyName = StringUtils::ToNative(_javascriptProperty->JavascriptName);
        auto clrPropertyName = _javascriptProperty->JavascriptName;

        if (_javascriptProperty->IsComplexType)
        {
            auto javascriptObjectWrapper = gcnew JavascriptObjectWrapper(_javascriptProperty->JsObject, _browserProcess);
            javascriptObjectWrapper->Bind(v8Value, callbackRegistry);

            _javascriptObjectWrapper = javascriptObjectWrapper;
        }
        else
        {
            auto propertyAttribute = _javascriptProperty->IsReadOnly ? V8_PROPERTY_ATTRIBUTE_READONLY : V8_PROPERTY_ATTRIBUTE_NONE;

            v8Value->SetValue(propertyName, V8_ACCESS_CONTROL_DEFAULT, propertyAttribute);
        }
    };
}
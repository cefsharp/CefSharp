// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptPropertyWrapper.h"
#include "JavascriptObjectWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptPropertyWrapper::Bind()
    {
        auto propertyName = StringUtils::ToNative(_javascriptProperty->JavascriptName);
        auto clrPropertyName = _javascriptProperty->JavascriptName;

        if (_javascriptProperty->IsComplexType)
        {
            auto javascriptObjectWrapper = gcnew JavascriptObjectWrapper(_javascriptProperty->JsObject, _browserProcess);
            javascriptObjectWrapper->V8Value = V8Value.get();
            javascriptObjectWrapper->Bind();

            _javascriptObjectWrapper = javascriptObjectWrapper;
        }
        else
        {
            auto propertyAttribute = _javascriptProperty->IsReadOnly ? V8_PROPERTY_ATTRIBUTE_READONLY : V8_PROPERTY_ATTRIBUTE_NONE;

            V8Value->SetValue(propertyName, V8_ACCESS_CONTROL_DEFAULT, propertyAttribute);
        }
    };
}
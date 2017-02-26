// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptMethodWrapper.h"

namespace CefSharp
{
    void JavascriptMethodWrapper::Bind(JavascriptMethod^ javascriptMethod, const CefRefPtr<CefV8Value>& v8Value)
    {
        _javascriptMethodName = javascriptMethod->JavascriptName;
        auto methodName = StringUtils::ToNative(javascriptMethod->JavascriptName);
        auto v8Function = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

        v8Value->SetValue(methodName, v8Function, V8_PROPERTY_ATTRIBUTE_NONE);
    };

    BrowserProcessResponse^ JavascriptMethodWrapper::Execute(array<Object^>^ parameters)
    {
        return _browserProcess->CallMethod(_ownerId, _javascriptMethodName, parameters);
    }
}
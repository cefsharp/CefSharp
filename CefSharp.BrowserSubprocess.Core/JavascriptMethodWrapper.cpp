﻿// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptMethodWrapper.h"
#include "JavascriptObjectWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptMethodWrapper::Bind()
    {
        auto methodName = StringUtils::ToNative(_javascriptMethod->JavascriptName);
        auto v8Function = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

        V8Value->SetValue(methodName, v8Function, V8_PROPERTY_ATTRIBUTE_NONE);
    };

    BrowserProcessResponse^ JavascriptMethodWrapper::Execute(array<Object^>^ parameters)
    {
        auto resp = _browserProcess->CallMethod(_ownerId, _javascriptMethod->JavascriptName, parameters);
        if (resp->Result->GetType() == JavascriptObject::typeid) {
            auto obj = safe_cast<JavascriptObject^>(resp->Result);
            auto j = gcnew JavascriptObjectWrapper(obj, _browserProcess);
            j->Bind();
            resp->Result = j;
        }
        return resp;
    }
}
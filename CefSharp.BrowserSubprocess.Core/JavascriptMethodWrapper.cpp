// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
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

    Object^ JavascriptMethodWrapper::WrapObject(JavascriptObject^ obj)
    {
        auto jsObjectWrapper = gcnew JavascriptObjectWrapper(obj, _browserProcess);
        jsObjectWrapper->Bind();
        return jsObjectWrapper;
    }

    Object^ JavascriptMethodWrapper::WrapArray(Array^ array)
    {
        CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(array->Length);
        for (int i = 0; i < array->Length; i++)
        {
            JavascriptObject^ jsObj = safe_cast<JavascriptObject^>(array->GetValue(i));
            if (jsObj == nullptr)
            {
                cefArray->SetValue(i, CefV8Value::CreateNull());
            }
            else
            {
                auto objWrapper = gcnew JavascriptObjectWrapper(jsObj, _browserProcess);
                objWrapper->Bind();
                cefArray->SetValue(i, objWrapper->V8Value.get());
            }
        }

        auto jsObjectWrapper = gcnew JavascriptObjectWrapper(nullptr, _browserProcess);
        jsObjectWrapper->V8Value = cefArray;
        return jsObjectWrapper;
    }

    BrowserProcessResponse^ JavascriptMethodWrapper::Execute(array<Object^>^ parameters)
    {
        auto response = _browserProcess->CallMethod(_ownerId, _javascriptMethod->JavascriptName, parameters);
        if (response->Success && response->Result != nullptr)
        {
            auto type = response->Result->GetType();
            if (type == JavascriptObject::typeid)
            {
                response->Result = WrapObject(safe_cast<JavascriptObject^>(response->Result));
            }
            else if (type->IsArray && type->GetElementType() == JavascriptObject::typeid)
            {
                response->Result = WrapArray(safe_cast<Array^>(response->Result));
            }
        }
        return response;
    }
}
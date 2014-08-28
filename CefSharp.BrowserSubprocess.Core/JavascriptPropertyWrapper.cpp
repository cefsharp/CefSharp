// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptPropertyWrapper.h"
#include "JavascriptObjectWrapper.h"
#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptPropertyWrapper::Bind()
    {
        auto methodName = StringUtils::ToNative(_javascriptProperty->JavascriptName);
        auto clrMethodName = _javascriptProperty->JavascriptName;

        if (_javascriptProperty->IsComplexType)
        {
            auto wrapperObject = gcnew JavascriptObjectWrapper(_javascriptProperty->Value);
            wrapperObject->V8Value = V8Value.get();
            wrapperObject->Bind();
        }
        else
        {
            V8Value->SetValue(methodName, V8_ACCESS_CONTROL_DEFAULT, V8_PROPERTY_ATTRIBUTE_NONE);
        }
    };

    void JavascriptPropertyWrapper::SetProperty(String^ memberName, Object^ value)
    {
        auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

        browserProxy->SetProperty(_ownerId, memberName, value);
    };

    Object^ JavascriptPropertyWrapper::GetProperty(String^ memberName)
    {
        auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

        return browserProxy->GetProperty(_ownerId, memberName);
    };
}
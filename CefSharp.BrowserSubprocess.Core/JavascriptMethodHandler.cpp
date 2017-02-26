// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "TypeUtils.h"
#include "JavascriptMethodHandler.h"

namespace CefSharp
{
    bool JavascriptMethodHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
    {
        auto parameter = gcnew array<Object^>(arguments.size());

        for (std::vector<CefRefPtr<CefV8Value>>::size_type i = 0; i != arguments.size(); i++)
        {
            parameter[i] = TypeUtils::ConvertFromCef(arguments[i], _callbackRegistry);
        }

        try
        {
            auto response = _method->Invoke(parameter);

            retval = ConvertToCefObject(response->Result);
            if (!response->Success)
            {
                exception = StringUtils::ToNative(response->Message);
            }
        }
        catch (Exception^ ex)
        {
            exception = StringUtils::ToNative(ex->ToString());
        }

        //NOTE: Return true otherwise exception is ignored
        return true;
    }

    CefRefPtr<CefV8Value> JavascriptMethodHandler::ConvertToCefObject(Object^ obj)
    {
        if (obj == nullptr)
        {
            return CefV8Value::CreateNull();
        }

        auto type = obj->GetType();

        if (type == JavascriptObject::typeid)
        {
            JavascriptObject^ javascriptObject = (JavascriptObject^)obj;
            CefRefPtr<CefV8Value> cefObject = CefV8Value::CreateObject(NULL, NULL);

            for (int i = 0; i < javascriptObject->Properties->Count; i++)
            {
                auto prop = javascriptObject->Properties[i];

                if (prop->IsComplexType)
                {
                    auto v8Value = ConvertToCefObject(prop->JsObject);

                    cefObject->SetValue(StringUtils::ToNative(prop->JavascriptName), v8Value, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
                }
                else
                {
                    auto v8Value = TypeUtils::ConvertToCef(prop->PropertyValue, nullptr);

                    cefObject->SetValue(StringUtils::ToNative(prop->JavascriptName), v8Value, CefV8Value::PropertyAttribute::V8_PROPERTY_ATTRIBUTE_NONE);
                }
            }

            return cefObject;
        }

        return TypeUtils::ConvertToCef(obj, nullptr);
    }
}
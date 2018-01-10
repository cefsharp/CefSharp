// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Primitives.h"
#include "JsObjectsSerialization.h"
#include "V8Serialization.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            void SerializeJsObject(JavascriptObject^ jsObject, const CefRefPtr<CefListValue> &list, int index)
            {
                auto objList = CefListValue::Create();
                SetInt64(objList, 0, jsObject->Id);
                objList->SetString(1, StringUtils::ToNative(jsObject->Name));
                objList->SetString(2, StringUtils::ToNative(jsObject->JavascriptName));
                objList->SetBool(3, jsObject->IsAsync);

                auto methodList = CefListValue::Create();
                auto j = 0;
                methodList->SetInt(j++, jsObject->Methods->Count);
                for each (JavascriptMethod^ jsMethod in jsObject->Methods)
                {
                    SetInt64(methodList, j++, jsMethod->Id);
                    methodList->SetString(j++, StringUtils::ToNative(jsMethod->ManagedName));
                    methodList->SetString(j++, StringUtils::ToNative(jsMethod->JavascriptName));
                    methodList->SetInt(j++, jsMethod->ParameterCount);
                }
                objList->SetList(4, methodList);

                auto propertyList = CefListValue::Create();
                j = 0;
                propertyList->SetInt(j++, jsObject->Properties->Count);
                for each(JavascriptProperty^ jsProperty in jsObject->Properties)
                {
                    SetInt64(propertyList, j++, jsProperty->Id);
                    propertyList->SetString(j++, StringUtils::ToNative(jsProperty->ManagedName));
                    propertyList->SetString(j++, StringUtils::ToNative(jsProperty->JavascriptName));
                    propertyList->SetBool(j++, jsProperty->IsComplexType);
                    propertyList->SetBool(j++, jsProperty->IsReadOnly);

                    if (jsProperty->JsObject != nullptr)
                    {
                        SerializeJsObject(jsProperty->JsObject, propertyList, j++);
                    }
                    else
                    {
                        propertyList->SetNull(j++);
                    }

                    if (jsProperty->PropertyValue != nullptr)
                    {
                        SerializeV8Object(propertyList, j++, jsProperty->PropertyValue);
                    }
                    else
                    {
                        propertyList->SetNull(j++);
                    }
                }
                objList->SetList(5, propertyList);

                list->SetList(index, objList);
            }

            void SerializeJsObjects(List<JavascriptObject^>^ objects, const CefRefPtr<CefListValue> &list, int index)
            {
                auto subList = CefListValue::Create();
                auto i = 0;
                for each (JavascriptObject^ jsObject in objects)
                {
                    SerializeJsObject(jsObject, subList, i++);
                }
                list->SetList(index, subList);
            }
        }
    }
}
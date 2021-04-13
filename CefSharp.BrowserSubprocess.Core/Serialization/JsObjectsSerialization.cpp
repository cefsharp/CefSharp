// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JsObjectsSerialization.h"
#include "../CefSharp.Core.Runtime/Internals/Serialization/Primitives.h"
#include "../CefSharp.Core.Runtime/Internals/Serialization/ObjectsSerialization.h"

using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        namespace Serialization
        {
            JavascriptObject^ DeserializeJsObject(const CefRefPtr<CefListValue>& rootList, int index)
            {
                if (rootList->GetType(index) == VTYPE_INVALID ||
                    rootList->GetType(index) == VTYPE_NULL)
                {
                    return nullptr;
                }

                auto list = rootList->GetList(index);
                auto jsObject = gcnew JavascriptObject();
                jsObject->Id = GetInt64(list, 0);
                jsObject->Name = StringUtils::ToClr(list->GetString(1));
                jsObject->JavascriptName = StringUtils::ToClr(list->GetString(2));
                jsObject->IsAsync = list->GetBool(3);

                auto methodList = list->GetList(4);
                auto methodCount = methodList->GetInt(0);
                auto k = 1;
                for (auto j = 0; j < methodCount; j++)
                {
                    auto jsMethod = gcnew JavascriptMethod();

                    jsMethod->Id = GetInt64(methodList, k++);
                    jsMethod->ManagedName = StringUtils::ToClr(methodList->GetString(k++));
                    jsMethod->JavascriptName = StringUtils::ToClr(methodList->GetString(k++));
                    jsMethod->ParameterCount = methodList->GetInt(k++);

                    jsObject->Methods->Add(jsMethod);
                }

                auto propertyList = list->GetList(5);
                auto propertyCount = propertyList->GetInt(0);
                k = 1;
                for (auto j = 0; j < propertyCount; j++)
                {
                    auto jsProperty = gcnew JavascriptProperty();

                    jsProperty->Id = GetInt64(propertyList, k++);
                    jsProperty->ManagedName = StringUtils::ToClr(propertyList->GetString(k++));
                    jsProperty->JavascriptName = StringUtils::ToClr(propertyList->GetString(k++));
                    jsProperty->IsComplexType = propertyList->GetBool(k++);
                    jsProperty->IsReadOnly = propertyList->GetBool(k++);

                    jsProperty->JsObject = DeserializeJsObject(propertyList, k++);
                    jsProperty->PropertyValue = DeserializeObject(propertyList, k++, nullptr);

                    jsObject->Properties->Add(jsProperty);
                }
                return jsObject;
            }

            List<JavascriptObject^>^ DeserializeJsObjects(const CefRefPtr<CefListValue>& list, int index)
            {
                auto result = gcnew List<JavascriptObject^>();
                auto subList = list->GetList(index);
                for (size_t i = 0; i < subList->GetSize(); i++)
                {
                    result->Add(DeserializeJsObject(subList, i));
                }

                return result;
            }
        }
    }
}

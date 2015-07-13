// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "ObjectsSerialization.h"
#include "../CefSharp.Core/Internals/Serialization/Primitives.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            JavascriptRootObject^ DeserializeJsObject(CefRefPtr<CefListValue> &list, int index)
            {
                auto result = gcnew JavascriptRootObject();
                auto subList = list->GetList(index);
                for (auto i = 0; i < subList->GetSize(); i++)
                {
                    auto objList = subList->GetList(i);
                    auto jsObject = gcnew JavascriptObject();
                    jsObject->Id = GetInt64(objList, 0);
                    jsObject->Name = StringUtils::ToClr(objList->GetString(1));
                    jsObject->JavascriptName = StringUtils::ToClr(objList->GetString(2));

                    auto methodList = objList->GetList(3);
                    auto methodCount = methodList->GetInt(0);
                    for (auto j = 0; j < methodCount; j++)
                    {
                        auto jsMethod = gcnew JavascriptMethod();

                        jsMethod->Id = GetInt64(methodList, j);
                        jsMethod->ManagedName = StringUtils::ToClr(methodList->GetString(j + 1));
                        jsMethod->JavascriptName = StringUtils::ToClr(methodList->GetString(j + 2));
                        jsMethod->ParameterCount = methodList->GetInt(j + 3);

                        jsObject->Methods->Add(jsMethod);
                    }

                    result->MemberObjects->Add(jsObject);
                }

                return result;
            }
        }
    }
}
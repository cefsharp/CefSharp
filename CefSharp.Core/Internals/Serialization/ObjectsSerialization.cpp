// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Primitives.h"
#include "ObjectsSerialization.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            void SerializeJsObject(JavascriptRootObject^ object, CefRefPtr<CefListValue> &list, int index)
            {
                auto subList = CefListValue::Create();
                auto i = 0;
                for each (JavascriptObject^ jsObject in object->MemberObjects)
                {
                    auto objList = CefListValue::Create();
                    SetInt64(jsObject->Id, objList, 0);
                    objList->SetString(1, StringUtils::ToNative(jsObject->Name));
                    objList->SetString(2, StringUtils::ToNative(jsObject->JavascriptName));

                    auto methodList = CefListValue::Create();
                    auto j = 0;
                    methodList->SetInt(j++, jsObject->Methods->Count);
                    for each (JavascriptMethod^ jsMethod in jsObject->Methods)
                    {
                        SetInt64(jsMethod->Id, methodList, j++);
                        methodList->SetString(j++, StringUtils::ToNative(jsMethod->ManagedName));
                        methodList->SetString(j++, StringUtils::ToNative(jsMethod->JavascriptName));
                        methodList->SetInt(j++, jsMethod->ParameterCount);
                    }
                    objList->SetList(3, methodList);

                    subList->SetList(i++, objList);
                }
                list->SetList(index, subList);
            }
        }
    }
}
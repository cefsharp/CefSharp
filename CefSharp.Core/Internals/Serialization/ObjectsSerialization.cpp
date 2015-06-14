#include "Stdafx.h"
#include "ObjectsSerialization.h"
#include "Primitives.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            void SerializeJsRootObject(JavascriptRootObject ^value, CefRefPtr<CefListValue> list)
            {
                auto memberObjects = value->MemberObjects;
                for (auto i = 0; i < memberObjects->Count; i++)
                {
                    SerializeJsObject(memberObjects[i], list, i);
                }
            }

            void SerializeJsObject(JavascriptObject ^value, CefRefPtr<CefListValue> list, int index)
            {
                auto val = CefListValue::Create();

                auto i = 0;
                SetInt64(value->Id, val, i++);
                val->SetString(i++, StringUtils::ToNative(value->Name));
                val->SetString(i++, StringUtils::ToNative(value->JavascriptName));

                val->SetInt(i++, value->Methods->Count);
                for each(auto method in value->Methods)
                {
                    SetInt64(method->Id, val, i++);
                    val->SetString(i++, StringUtils::ToNative(method->JavascriptName));
                    val->SetString(i++, StringUtils::ToNative(method->ManagedName));
                    val->SetInt(i++, method->ParameterCount);
                }

                val->SetInt(i++, value->Properties->Count);
                for each(auto property in value->Properties)
                {
                    SetInt64(property->Id, val, i++);
                    val->SetString(i++, StringUtils::ToNative(property->JavascriptName));
                    val->SetString(i++, StringUtils::ToNative(property->ManagedName));
                    val->SetBool(i++, property->IsComplexType);
                    val->SetBool(i++, property->IsReadOnly);
                    if (property->IsComplexType)
                    {
                        if (property->JsObject != nullptr)
                        {
                            SerializeJsObject(property->JsObject, val, i++);
                        }
                        else
                        {
                            val->SetNull(i++);
                        }
                    }
                }
                
                list->SetList(index, val);
            }
        }
    }
}
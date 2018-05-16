// Copyright � 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "include\cef_values.h"

#include "V8Serialization.h"
#include "Primitives.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            template<typename TList, typename TIndex>
            void SerializeV8Object(const CefRefPtr<TList>& list, const TIndex& index, Object^ obj)
            {
                // Collection of ancestors to currently serialised object.
                // This enables prevention of endless loops due to cycles in graphs where
                // a child references one of its ancestors.
                auto ancestors = gcnew HashSet<Object^>();
                SerializeV8SimpleObject(list, index, obj, ancestors);
            }

            template<typename TList, typename TIndex>
            void SerializeV8SimpleObject(const CefRefPtr<TList>& list, const TIndex& index, Object^ obj, HashSet<Object^>^ ancestors)
            {
                list->SetNull(index);

                if (obj == nullptr || ancestors->Contains(obj))
                {
                    return;
                }
                ancestors->Add(obj);

                auto type = obj->GetType();
                Type^ underlyingType = Nullable::GetUnderlyingType(type);
                if (underlyingType != nullptr) type = underlyingType;

                if (type == Boolean::typeid)
                {
                    list->SetBool(index, safe_cast<bool>(obj));
                }
                else if (type == Int32::typeid)
                {
                    list->SetInt(index, safe_cast<int>(obj));
                }
                else if (type == String::typeid)
                {
                    list->SetString(index, StringUtils::ToNative(safe_cast<String^>(obj)));
                }
                else if (type == Double::typeid)
                {
                    list->SetDouble(index, safe_cast<double>(obj));
                }
                else if (type == Decimal::typeid)
                {
                    list->SetDouble(index, Convert::ToDouble(obj));
                }
                else if (type == SByte::typeid)
                {
                    list->SetInt(index, Convert::ToInt32(obj));
                }
                else if (type == Int16::typeid)
                {
                    list->SetInt(index, Convert::ToInt32(obj));
                }
                else if (type == Int64::typeid)
                {
                    list->SetDouble(index, Convert::ToDouble(obj));
                }
                else if (type == Byte::typeid)
                {
                    list->SetInt(index, Convert::ToInt32(obj));
                }
                else if (type == UInt16::typeid)
                {
                    list->SetInt(index, Convert::ToInt32(obj));
                }
                else if (type == UInt32::typeid)
                {
                    list->SetDouble(index, Convert::ToDouble(obj));
                }
                else if (type == UInt64::typeid)
                {
                    list->SetDouble(index, Convert::ToDouble(obj));
                }
                else if (type == Single::typeid)
                {
                    list->SetDouble(index, Convert::ToDouble(obj));
                }
                else if (type == Char::typeid)
                {
                    list->SetInt(index, Convert::ToInt32(obj));
                }
                else if (type == DateTime::typeid)
                {
                    SetCefTime(list, index, ConvertDateTimeToCefTime(safe_cast<DateTime>(obj)));
                }
                // Serialize dictionary to CefDictionary (key,value pairs)
                else if (System::Collections::IDictionary::typeid->IsAssignableFrom(type))
                {
                    auto subDict = CefDictionaryValue::Create();
                    auto dict = (System::Collections::IDictionary^) obj;
                    for each (System::Collections::DictionaryEntry kvp in dict)
                    {
                        auto fieldName = StringUtils::ToNative(Convert::ToString(kvp.Key));
                        SerializeV8SimpleObject(subDict, fieldName, kvp.Value, ancestors);
                    }
                    list->SetDictionary(index, subDict);
                }
                else if (System::Collections::IEnumerable::typeid->IsAssignableFrom(type))
                {
                    auto subList = CefListValue::Create();
                    auto enumerable = (System::Collections::IEnumerable^) obj;

                    int i = 0;
                    for each (Object^ arrObj in enumerable)
                    {
                        SerializeV8SimpleObject(subList, i, arrObj, ancestors);
                        i++;
                    }
                    list->SetList(index, subList);
                }
                // Serialize class/structs to CefDictionary (key,value pairs)
                else if (!type->IsPrimitive && !type->IsEnum)
                {
                    auto fields = type->GetFields();
                    auto subDict = CefDictionaryValue::Create();

                    for (int i = 0; i < fields->Length; i++)
                    {
                        auto fieldName = StringUtils::ToNative(fields[i]->Name);
                        auto fieldValue = fields[i]->GetValue(obj);
                        SerializeV8SimpleObject(subDict, fieldName, fieldValue, ancestors);
                    }

                    auto properties = type->GetProperties();

                    for (int i = 0; i < properties->Length; i++)
                    {
                        auto propertyName = StringUtils::ToNative(properties[i]->Name);
                        auto propertyValue = properties[i]->GetValue(obj);
                        SerializeV8SimpleObject(subDict, propertyName, propertyValue, ancestors);
                    }
                    list->SetDictionary(index, subDict);
                }
                else
                {
                    throw gcnew NotSupportedException("Unable to serialize Type");
                }

                ancestors->Remove(obj);
            }

            CefTime ConvertDateTimeToCefTime(DateTime dateTime)
            {
                auto timeSpan = dateTime - DateTime(1970, 1, 1);

                return CefTime(timeSpan.TotalSeconds);
            }
        }
    }
}
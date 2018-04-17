// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
                auto seen = gcnew Stack<Object^>();
                SerializeV8SimpleObject(list, index, obj, seen);
            }

            template<typename TList, typename TIndex>
            void SerializeV8SimpleObject(const CefRefPtr<TList>& list, const TIndex& index, Object^ obj, Stack<Object^>^ seen)
            {
                list->SetNull(index);

                if (obj == nullptr || seen->Contains(obj))
                {
                    return;
                }
                seen->Push(obj);

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
                else if (type->IsArray)
                {
                    auto subList = CefListValue::Create();
                    Array^ managedArray = (Array^)obj;
                    for (int i = 0; i < managedArray->Length; i++)
                    {
                        Object^ arrObj;
                        arrObj = managedArray->GetValue(i);
                        SerializeV8SimpleObject(subList, i, arrObj, seen);
                    }
                    list->SetList(index, subList);
                }
                // Serialize dictionary to CefDictionary (key,value pairs)
                else if (System::Collections::IDictionary::typeid->IsAssignableFrom(type))
                {
                    auto subDict = CefDictionaryValue::Create();
                    auto dict = (System::Collections::IDictionary^) obj;
                    for each (System::Collections::DictionaryEntry kvp in dict)
                    {
                        auto fieldName = StringUtils::ToNative(Convert::ToString(kvp.Key));
                        SerializeV8SimpleObject(subDict, fieldName, kvp.Value, seen);
                    }
                    list->SetDictionary(index, subDict);
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
                        SerializeV8SimpleObject(subDict, fieldName, fieldValue, seen);
                    }

                    auto properties = type->GetProperties();

                    for (int i = 0; i < properties->Length; i++)
                    {
                        auto propertyName = StringUtils::ToNative(properties[i]->Name);
                        auto propertyValue = properties[i]->GetValue(obj);
                        SerializeV8SimpleObject(subDict, propertyName, propertyValue, seen);
                    }
                    list->SetDictionary(index, subDict);
                } 
                else
                {
                    throw gcnew NotSupportedException("Unable to serialize Type");
                }

                seen->Pop();
            }

            CefTime ConvertDateTimeToCefTime(DateTime dateTime)
            {
                auto timeSpan = dateTime - DateTime(1970, 1, 1);

                return CefTime(timeSpan.TotalSeconds);
            }
        }
    }
}
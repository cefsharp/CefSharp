// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
            void SerializeV8SimpleObject(Object^ obj, CefRefPtr<TList> list, TIndex index, Stack<Object^>^ seen);

            template<typename TList, typename TIndex>
            void SerializeV8Object(Object^ obj, CefRefPtr<TList> list, TIndex index)
            {
                auto seen = gcnew Stack<Object^>();
                SerializeV8SimpleObject(obj, list, index, seen);
            }

            template<typename TList, typename TIndex>
            void SerializeV8SimpleObject(Object^ obj, CefRefPtr<TList> list, TIndex index, Stack<Object^>^ seen)
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
                    SetCefTime(ConvertDateTimeToCefTime(safe_cast<DateTime>(obj)), list, index);
                }
                else if (type->IsArray)
                {
                    auto subList = CefListValue::Create();
                    Array^ managedArray = (Array^)obj;
                    for (int i = 0; i < managedArray->Length; i++)
                    {
                        Object^ arrObj;
                        arrObj = managedArray->GetValue(i);
                        SerializeV8SimpleObject(arrObj, subList, i, seen);
                    }
                    list->SetList(index, subList);
                }
                else if (type->IsValueType && !type->IsPrimitive && !type->IsEnum)
                {
                    auto fields = type->GetFields();
                    auto subDict = CefDictionaryValue::Create();

                    for (int i = 0; i < fields->Length; i++)
                    {
                        auto fieldName = fields[i]->Name;
                        auto strFieldName = StringUtils::ToNative(safe_cast<String^>(fieldName));
                        auto fieldVal = fields[i]->GetValue(obj);
                        SerializeV8SimpleObject(fieldVal, subDict, strFieldName, seen);
                    }
                } 
                else
                {
                    throw gcnew NotSupportedException("Complex types cannot be serialized to Cef lists");
                }

                seen->Pop();
            }

            CefTime ConvertDateTimeToCefTime(DateTime dateTime)
            {
                auto timeSpan = dateTime - DateTime(1970, 1, 1);

                return CefTime(timeSpan.TotalSeconds);
            }

            template void SerializeV8Object(Object^ obj, CefRefPtr<CefListValue> list, int index);
            template void SerializeV8Object(Object^ obj, CefRefPtr<CefDictionaryValue> list, CefString index);
        }
    }
}
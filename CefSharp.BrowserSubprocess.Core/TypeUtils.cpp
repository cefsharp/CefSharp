// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include ".\..\CefSharp.Core\Internals\StringUtils.h"
#include "TypeUtils.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    CefRefPtr<CefV8Value> TypeUtils::ConvertToCef(Object^ obj, Type^ type)
    {
        if (type == Void::typeid)
        {
            return CefV8Value::CreateUndefined();
        }
        if (obj == nullptr)
        {
            return CefV8Value::CreateNull();
        }

        if (type == nullptr)
        {
            type = obj->GetType();
        }

        Type^ underlyingType = Nullable::GetUnderlyingType(type);
        if (underlyingType != nullptr) type = underlyingType;

        if (type == Boolean::typeid)
        {
            return CefV8Value::CreateBool(safe_cast<bool>(obj));
        }
        if (type == Int32::typeid)
        {
            return CefV8Value::CreateInt(safe_cast<int>(obj));
        }
        if (type == String::typeid)
        {
            CefString str = StringUtils::ToNative(safe_cast<String^>(obj));
            return CefV8Value::CreateString(str);
        }
        if (type == Double::typeid)
        {
            return CefV8Value::CreateDouble(safe_cast<double>(obj));
        }
        if (type == Decimal::typeid)
        {
            return CefV8Value::CreateDouble(Convert::ToDouble(obj));
        }
        if (type == SByte::typeid)
        {
            return CefV8Value::CreateInt(Convert::ToInt32(obj));
        }
        if (type == Int16::typeid)
        {
            return CefV8Value::CreateInt(Convert::ToInt32(obj));
        }
        if (type == Int64::typeid)
        {
            return CefV8Value::CreateDouble(Convert::ToDouble(obj));
        }
        if (type == Byte::typeid)
        {
            return CefV8Value::CreateInt(Convert::ToInt32(obj));
        }
        if (type == UInt16::typeid)
        {
            return CefV8Value::CreateInt(Convert::ToInt32(obj));
        }
        if (type == UInt32::typeid)
        {
            return CefV8Value::CreateDouble(Convert::ToDouble(obj));
        }
        if (type == UInt64::typeid)
        {
            return CefV8Value::CreateDouble(Convert::ToDouble(obj));
        }
        if (type == Single::typeid)
        {
            return CefV8Value::CreateDouble(Convert::ToDouble(obj));
        }
        if (type == Char::typeid)
        {
            return CefV8Value::CreateInt(Convert::ToInt32(obj));
        }
        if (type == DateTime::typeid)
        {
            return CefV8Value::CreateDate(TypeUtils::ConvertDateTimeToCefTime(safe_cast<DateTime>(obj)));
        }
        if (type->IsArray)
        {
            Array^ managedArray = (Array^)obj;
            CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(managedArray->Length);

            for (int i = 0; i < managedArray->Length; i++)
            {
                Object^ arrObj;

                arrObj = managedArray->GetValue(i);

                if (arrObj != nullptr)
                {
                    CefRefPtr<CefV8Value> cefObj = TypeUtils::ConvertToCef(arrObj, arrObj->GetType());

                    cefArray->SetValue(i, cefObj);
                }
                else
                {
                    cefArray->SetValue(i, CefV8Value::CreateNull());
                }
            }

            return cefArray;
        }
        if (type->IsValueType && !type->IsPrimitive && !type->IsEnum)
        {
            cli::array<System::Reflection::FieldInfo^>^ fields = type->GetFields();
            CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(fields->Length);

            for (int i = 0; i < fields->Length; i++)
            {
                String^ fieldName = fields[i]->Name;

                CefString strFieldName = StringUtils::ToNative(safe_cast<String^>(fieldName));

                Object^ fieldVal = fields[i]->GetValue(obj);

                if (fieldVal != nullptr)
                {
                    CefRefPtr<CefV8Value> cefVal = TypeUtils::ConvertToCef(fieldVal, fieldVal->GetType());

                    cefArray->SetValue(strFieldName, cefVal, V8_PROPERTY_ATTRIBUTE_NONE);
                }
                else
                {
                    cefArray->SetValue(strFieldName, CefV8Value::CreateNull(), V8_PROPERTY_ATTRIBUTE_NONE);
                }
            }

            return cefArray;
        }
        //TODO: What exception type?
        throw gcnew Exception(String::Format("Cannot convert '{0}' object from CLR to CEF.", type->FullName));
    }

    Object^ TypeUtils::ConvertFromCef(CefRefPtr<CefV8Value> obj, JavascriptCallbackRegistry^ callbackRegistry)
    {
        if (obj->IsNull() || obj->IsUndefined())
        {
            return nullptr;
        }

        if (obj->IsBool())
        {
            return gcnew System::Boolean(obj->GetBoolValue());
        }
        if (obj->IsInt())
        {
            return gcnew System::Int32(obj->GetIntValue());
        }
        if (obj->IsDouble())
        {
            return gcnew System::Double(obj->GetDoubleValue());
        }
        if (obj->IsString())
        {
            return StringUtils::ToClr(obj->GetStringValue());
        }
        if (obj->IsDate())
        {
            return TypeUtils::ConvertCefTimeToDateTime(obj->GetDateValue());
        }

        if (obj->IsArray())
        {
            int arrLength = obj->GetArrayLength();

            if (arrLength > 0)
            {
                std::vector<CefString> keys;
                if (obj->GetKeys(keys))
                {
                    auto array = gcnew List<Object^>();

                    for (int i = 0; i < arrLength; i++)
                    {
                        auto data = obj->GetValue(keys[i]);
                        if (data != nullptr)
                        {
                            auto p_data = TypeUtils::ConvertFromCef(data, callbackRegistry);

                            array->Add(p_data);
                        }
                    }

                    return array->ToArray();
                }
            }

            return nullptr;
        }

        if (obj->IsFunction())
        {
            if (callbackRegistry == nullptr)
            {
                return nullptr;
            }

            return callbackRegistry->Register(CefV8Context::GetCurrentContext(), obj);
        }

        if (obj->IsObject())
        {
            std::vector<CefString> keys;
            if (obj->GetKeys(keys))
            {
                int objLength = keys.size();
                if (objLength > 0)
                {
                    auto result = gcnew Dictionary<String^, Object^>();

                    for (int i = 0; i < objLength; i++)
                    {
                        String^ p_keyStr = StringUtils::ToClr(keys[i].ToString());

                        if ((obj->HasValue(keys[i])) && (!p_keyStr->StartsWith("__")))
                        {
                            CefRefPtr<CefV8Value> data = obj->GetValue(keys[i]);
                            if (data != nullptr)
                            {
                                Object^ p_data = TypeUtils::ConvertFromCef(data, callbackRegistry);

                                result->Add(p_keyStr, p_data);
                            }
                        }
                    }

                    return result;
                }
            }

            return nullptr;
        }

        //TODO: What exception type?
        throw gcnew Exception("Cannot convert object from Cef to CLR.");
    }

    DateTime TypeUtils::ConvertCefTimeToDateTime(CefTime time)
    {
        return DateTimeUtils::FromCefTime(time.year,
            time.month,
            time.day_of_month,
            time.hour,
            time.minute,
            time.second,
            time.millisecond);
    }

    CefTime TypeUtils::ConvertDateTimeToCefTime(DateTime dateTime)
    {
        return CefTime(DateTimeUtils::ToCefTime(dateTime));
    }
}
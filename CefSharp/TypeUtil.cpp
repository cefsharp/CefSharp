#include "Stdafx.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    CefRefPtr<CefV8Value> convertToCef(Object^ obj, Type^ type)
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
        if(underlyingType != nullptr) type = underlyingType;

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
            CefString str = toNative(safe_cast<String^>(obj));
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
                    CefRefPtr<CefV8Value> cefObj = convertToCef(arrObj, arrObj->GetType());

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
            CefRefPtr<CefV8Value> cefObj = CefV8Value::CreateObject(NULL);

            for (int i = 0; i < fields->Length; i++)
            {
                String^ fieldName = fields[i]->Name;

                CefString strFieldName = toNative(safe_cast<String^>(fieldName));

                Object^ fieldVal = fields[i]->GetValue(obj);

                if (fieldVal != nullptr)
                {
                    CefRefPtr<CefV8Value> cefVal = convertToCef(fieldVal, fieldVal->GetType());

                    cefObj->SetValue(strFieldName, cefVal, V8_PROPERTY_ATTRIBUTE_NONE);
                }
                else
                {
                    cefObj->SetValue(strFieldName, CefV8Value::CreateNull(), V8_PROPERTY_ATTRIBUTE_NONE);
                }
            }

            return cefObj;
        }
        //TODO: What exception type?
        throw gcnew Exception(String::Format("Cannot convert '{0}' object from CLR to CEF.", type->FullName));
    }

    System::String^ stdToString(const std::string& s)
    {
        return gcnew System::String(s.c_str());
    }

    Object^ convertFromCef(CefRefPtr<CefV8Value> obj)
    {
        if (obj->IsNull() || obj->IsUndefined())
        {
            return nullptr;
        }

        if (obj->IsBool())
            return gcnew System::Boolean(obj->GetBoolValue());
        if (obj->IsInt())
            return gcnew System::Int32(obj->GetIntValue());
        if (obj->IsDouble())
            return gcnew System::Double(obj->GetDoubleValue());
        if (obj->IsString())
            return toClr(obj->GetStringValue());

        if (obj->IsArray())
        {
            int arrLength = obj->GetArrayLength();

            if (arrLength > 0)
            {
                std::vector<CefString> keys;
                if (obj->GetKeys(keys))
                {
                    Dictionary<String^, Object^>^ result = gcnew Dictionary<String^, Object^>();

                    for (int i = 0; i < arrLength; i++)
                    {
                        std::string p_key = keys[i].ToString();
                        String^ p_keyStr = stdToString(p_key);

                        if ((obj->HasValue(keys[i])) && (!p_keyStr->StartsWith("__")))
                        {
                            CefRefPtr<CefV8Value> data;

                            data = obj->GetValue(keys[i]);
                            if (data != nullptr)
                            {
                                Object^ p_data = convertFromCef(data);

                                result->Add(p_keyStr, p_data);
                            }
                        }
                    }

                    return result;
                }
            }

            return nullptr;
        }

        if (obj->IsObject())
        {
            std::vector<CefString> keys;
            if (obj->GetKeys(keys))
            {
                int objLength = keys.size();
                if (objLength > 0)
                {
                    Dictionary<String^, Object^>^ result = gcnew Dictionary<String^, Object^>();

                    for (int i = 0; i < objLength; i++)
                    {
                        std::string p_key = keys[i].ToString();
                        String^ p_keyStr = stdToString(p_key);

                        if ((obj->HasValue(keys[i])) && (!p_keyStr->StartsWith("__")))
                        {
                            CefRefPtr<CefV8Value> data;

                            data = obj->GetValue(keys[i]);
                            if (data != nullptr)
                            {
                                Object^ p_data = convertFromCef(data);

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
}
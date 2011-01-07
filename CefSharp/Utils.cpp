#include "stdafx.h"

namespace CefSharp
{
    String^ toClr(const cef_string_t& cefStr)
    {
        return gcnew String(cefStr.str);
    }
    
    String^ toClr(const CefString& cefStr)
    {
        return gcnew String(cefStr.c_str());
    }

    CefString toNative(String^ str)
    {
        pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
        CefString cefStr(pStr);
        return cefStr;
    }

    void assignFromString(cef_string_t& cefStrT, String^ str)
    {
        cef_string_clear(&cefStrT);
        if(str != nullptr)
        {
            pin_ptr<const wchar_t> pStr = PtrToStringChars(str);
            cef_string_copy(pStr, str->Length, &cefStrT);
        }
    }

    CefRefPtr<CefV8Value> convertToCef(Object^ obj, Type^ type)
    {
        if(type == Void::typeid)
        {
            return CefV8Value::CreateUndefined();
        }
        if(obj == nullptr)
        {
            return CefV8Value::CreateNull();
        }

        Type^ underlyingType = Nullable::GetUnderlyingType(type);
        if(underlyingType!=nullptr)type = underlyingType;

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
            return CefV8Value::CreateDouble( Convert::ToDouble(obj) );
        }
        if (type == SByte::typeid)
        {
            return CefV8Value::CreateInt( Convert::ToInt32(obj) );
        }
        if (type == Int16::typeid)
        {
            return CefV8Value::CreateInt( Convert::ToInt32(obj) );
        }
        if (type == Int64::typeid)
        {
            return CefV8Value::CreateDouble( Convert::ToDouble(obj) );
        }
        if (type == Byte::typeid)
        {
            return CefV8Value::CreateInt( Convert::ToInt32(obj) );
        }
        if (type == UInt16::typeid)
        {
            return CefV8Value::CreateInt( Convert::ToInt32(obj) );
        }
        if (type == UInt32::typeid)
        {
            return CefV8Value::CreateDouble( Convert::ToDouble(obj) );
        }
        if (type == UInt64::typeid)
        {
            return CefV8Value::CreateDouble( Convert::ToDouble(obj) );
        }
        if (type == Single::typeid)
        {
            return CefV8Value::CreateDouble( Convert::ToDouble(obj) );
        }
        if (type == Char::typeid)
        {
            return CefV8Value::CreateInt( Convert::ToInt32(obj) );
        }

        //TODO: What exception type?
        throw gcnew Exception("Cannot convert object from CLR to Cef " + type->ToString() + ".");
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
        if (obj->IsString()){
            return toClr(obj->GetStringValue());
        }

        //TODO: What exception type?
        throw gcnew Exception("Cannot convert object from Cef to CLR.");
    }

}
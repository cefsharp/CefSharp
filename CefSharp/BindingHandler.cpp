#include "stdafx.h"
#include "BindingHandler.h"

namespace CefSharp
{
    bool BindingHandler::IsNullableType(Type^ type)
    {
        // This is check traditionaly perform by this C# code:
        // return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        // But we have some problems with Nullable<>::typeid.
        return Nullable::GetUnderlyingType(type) != nullptr;
    }

    /// <summary></summary>
    /// <return>Returns conversion cost, or -1 if no conversion available.</return>
    int BindingHandler::GetChangeTypeCost(Object^ value, Type^ conversionType)
    {
        // TODO: temporary Int64 support fully disabled
        if (conversionType == Int64::typeid 
            || conversionType == Nullable<Int64>::typeid
            || conversionType == UInt64::typeid 
            || conversionType == Nullable<UInt64>::typeid
            )
            return -1;

        // Null conversion
        if (value == nullptr)
        {
            // TODO: This is check for reference type, may be not accuracy.
            if (conversionType->IsValueType == false) return 0;

            // Nullable types also can be converted to null without penalty.
            if (IsNullableType(conversionType)) return 0;

            // Non-reference and non-nullable types can not be converted from null.
            return -1;
        }

        // value is not null

        // value have same type - no conversion required
        Type^ valueType = value->GetType();
        if (valueType == conversionType) return 0;

        int baseCost = 0;

        // but conversionType can be Nullable
        Type^ targetType = Nullable::GetUnderlyingType(conversionType);
        if (targetType != nullptr)
        {
            // this is a nullable type, and it cost + 1
            conversionType = targetType;
            baseCost++;
        }
        if (valueType == conversionType) return baseCost + 0;

        if (valueType == Boolean::typeid)
        {
            // Boolean can be converted only to Boolean
            if(conversionType == Boolean::typeid) return baseCost + 0;
            return -1;
        }
        else if (valueType == Int32::typeid)
        {
            int int32Val = safe_cast<int>(value);

            if (conversionType == Int32::typeid) return baseCost + 0;
            else if (conversionType == UInt32::typeid && (int32Val >= 0)) return baseCost + 1;
            else if (conversionType == Int16::typeid && (int32Val >= Int16::MinValue && int32Val <= Int16::MaxValue)) return baseCost + 2;
            else if (conversionType == UInt16::typeid && (int32Val >= UInt16::MinValue && int32Val <= UInt16::MaxValue)) return baseCost + 3;
            else if (conversionType == Char::typeid && (int32Val >= Char::MinValue && int32Val <= Char::MaxValue)) return baseCost + 4;
            else if (conversionType == SByte::typeid && (int32Val >= SByte::MinValue && int32Val <= SByte::MaxValue)) return baseCost + 5;
            else if (conversionType == Byte::typeid && (int32Val >= Byte::MinValue && int32Val <= Byte::MaxValue)) return baseCost + 6;
            else if (conversionType == Double::typeid) return baseCost + 9;
            else if (conversionType == Single::typeid) return baseCost + 10;
            else if (conversionType == Decimal::typeid) return baseCost + 11;
            else if (conversionType == Int64::typeid) return -1;
            else if (conversionType == UInt64::typeid) return -1;
            return -1;
        }
        else if(valueType == Double::typeid)
        {
            double doubleVal = safe_cast<double>(value);

            if (conversionType == Double::typeid) return baseCost + 0;
            else if (conversionType == Single::typeid && (doubleVal >= Single::MinValue && doubleVal <= Single::MaxValue)) return baseCost + 1;
            else if (conversionType == Decimal::typeid /* && (doubleVal >= Decimal::MinValue && doubleVal <= Decimal::MaxValue) */) return baseCost + 2;
            else if (conversionType == Int32::typeid && (doubleVal >= Int32::MinValue && doubleVal <= Int32::MaxValue)) return baseCost + 3;
            else if (conversionType == UInt32::typeid && (doubleVal >= UInt32::MinValue && doubleVal <= UInt32::MaxValue)) return baseCost + 4;
            else if (conversionType == Int16::typeid && (doubleVal >= Int16::MinValue && doubleVal <= Int16::MaxValue)) return baseCost + 5;
            else if (conversionType == UInt16::typeid && (doubleVal >= UInt16::MinValue && doubleVal <= UInt16::MaxValue)) return baseCost + 6;
            else if (conversionType == Char::typeid && (doubleVal >= Char::MinValue && doubleVal <= Char::MaxValue)) return baseCost + 6;
            else if (conversionType == SByte::typeid && (doubleVal >= SByte::MinValue && doubleVal <= SByte::MaxValue)) return baseCost + 8;
            else if (conversionType == Byte::typeid && (doubleVal >= Byte::MinValue && doubleVal <= Byte::MaxValue)) return baseCost + 9;
            else if (conversionType == Int64::typeid) return -1;
            else if (conversionType == UInt64::typeid) return -1;
            return -1;
        }
        else if(valueType == String::typeid)
        {
            // String can be converted only to String
            if(conversionType == String::typeid) return baseCost + 0;
            return -1;
        }
        else
        {
            // No conversion available
            return -1;
        }
    }

    Object^ BindingHandler::ChangeType(Object^ value, Type^ conversionType)
    {
        if (GetChangeTypeCost(value, conversionType) < 0)
        {
            throw gcnew Exception("No conversion available.");
        }

        if (value == nullptr) return nullptr;

        Type^ targetType = Nullable::GetUnderlyingType(conversionType);
        if (targetType != nullptr) conversionType = targetType;

        return Convert::ChangeType(value, conversionType);
    }

    bool BindingHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
    {
        CefRefPtr<BindingData> bindingData = static_cast<BindingData*>(object->GetUserData().get());
        Object^ self = bindingData->Get();
        if(self == nullptr) 
        {
            exception = "Binding's CLR object is null.";
            return true;
        }

        String^ memberName = toClr(name);
        Type^ type = self->GetType();
        array<System::Reflection::MemberInfo^>^ members = type->GetMember(memberName, MemberTypes::Method, 
            /* BindingFlags::IgnoreCase |*/ BindingFlags::Instance | BindingFlags::Public);

        if(members->Length == 0)
        {
            exception = toNative("No member named " + memberName + ".");
            return true;
        }

        //TODO: cache for type info here

        array<System::Object^>^ suppliedArguments = gcnew array<Object^>(arguments.size());
        try
        {
            for(int i = 0; i < suppliedArguments->Length; i++) 
            {
                suppliedArguments[i] = convertFromCef(arguments[i]);
            }
        }
        catch(System::Exception^ err)
        {
            exception = toNative(err->Message);
            return true;
        }

        // choose best method
        MethodInfo^ bestMethod;
        array<Object^>^ bestMethodArguments;
        int bestMethodCost = -1;

        for (int i = 0; i < members->Length; i++)
        {
            MethodInfo^ method = (MethodInfo^) members[i];
            array<ParameterInfo^>^ parametersInfo = method->GetParameters();
            array<Object^>^ arguments;

            if (suppliedArguments->Length == parametersInfo->Length)
            {
                int failed = 0;
                int cost = 0;
                arguments = gcnew array<Object^>(suppliedArguments->Length);

                try
                {
                    for (int p = 0; p < suppliedArguments->Length; p++)
                    {
                        System::Type^ paramType = parametersInfo[p]->ParameterType;

                        int paramCost = GetChangeTypeCost(suppliedArguments[p], paramType);
                        if (paramCost < 0 )
                        {
                            failed++;
                            break;
                        }
                        else
                        {
                            arguments[p] = ChangeType(suppliedArguments[p], paramType);
                            cost += paramCost;
                        }
                    }
                }
                catch(Exception^)
                {
                    failed++;
                }

                if (failed > 0)
                {
                    continue;
                }

                if (cost < bestMethodCost || bestMethodCost < 0)
                {
                    bestMethod = method;
                    bestMethodArguments = arguments;
                    bestMethodCost = cost;
                }

                // this is best as possible cost
                if (cost == 0)
                    break;
            }
        }

        if (bestMethod != nullptr)
        {
            try
            {
                Object^ result = bestMethod->Invoke(self, bestMethodArguments);
                retval = convertToCef(result, bestMethod->ReturnType);
                return true;
            }
            catch(System::Reflection::TargetInvocationException^ err)
            {
                exception = toNative(err->InnerException->Message);
            }
            catch(System::Exception^ err)
            {
                exception = toNative(err->Message);
            }
        }
        else
        {
            exception = toNative("Argument mismatch for method \"" + memberName + "\".");
        }
        return true;
    }

    void BindingHandler::Bind(String^ name, Object^ obj, CefRefPtr<CefV8Value> window)
    {
        CefRefPtr<BindingData> bindingData = new BindingData(obj);
        CefRefPtr<CefBase> userData = static_cast<CefRefPtr<CefBase>>(bindingData);
        CefRefPtr<CefV8Value> wrappedObject = window->CreateObject(userData, NULL);
        CefRefPtr<CefV8Handler> handler = static_cast<CefV8Handler*>(new BindingHandler());

        array<MethodInfo^>^ methods = obj->GetType()->GetMethods(BindingFlags::Instance | BindingFlags::Public);

        IList<String^>^ methodNames = gcnew List<String^>();
        for each(MethodInfo^ method in methods) 
        {
            if(!methodNames->Contains(method->Name))
            {
                methodNames->Add(method->Name);
            }
        }

        for each(String^ methodName in methodNames)
        {
            CefString nameStr = toNative(methodName);
            wrappedObject->SetValue(nameStr, CefV8Value::CreateFunction(nameStr, handler), V8_PROPERTY_ATTRIBUTE_NONE);
        }

        window->SetValue(toNative(name), wrappedObject, V8_PROPERTY_ATTRIBUTE_NONE);
    }
}
#include "Stdafx.h"
#include "UnmanagedWrapper.h"
#include "BindingHandler.h"
#include "PropertyAccessor.h"

using namespace System::Reflection;

namespace CefSharp
{
    namespace Internals
    {
        namespace JavascriptBinding
        {
            void BindingHandler::Bind(String^ name, Object^ obj, CefRefPtr<CefV8Value> window)
            {
                window->SetValue(toNative(name), Bind(obj, window), V8_PROPERTY_ATTRIBUTE_NONE);
            }

            CefRefPtr<CefV8Value> BindingHandler::Bind(Object^ obj, CefRefPtr<CefV8Value> window)
            {
                IDictionary<Object^, unsigned long>^ $ = cache;
                cacheLock->EnterReadLock();
                try {
                    if (cache->ContainsKey(obj)) 
                        return map4cache[$[obj]];
                }
                finally {
                    cacheLock->ExitReadLock();
                }

                auto unmanagedWrapper = new UnmanagedWrapper(obj);

                // Create the Javascript/V8 object and associate it with the wrapped object.
                static auto propertyAccessor = new PropertyAccessor();
                auto javascriptWrapper = window->CreateObject(static_cast<CefRefPtr<CefV8Accessor>>(propertyAccessor));
                javascriptWrapper->SetUserData(static_cast<CefRefPtr<CefBase>>(unmanagedWrapper));

                static auto handler = static_cast<CefV8Handler*>(new BindingHandler());
                CreateJavascriptMethods(handler, javascriptWrapper, GetMethodNames(obj->GetType()));
                CreateJavascriptProperties(javascriptWrapper, GetProperties(obj->GetType()));

                static unsigned long key = -1;
                unsigned long k = CefAtomicIncrement(&key);
                if (InterlockedCompareExchange(&key, -1, -1) == -1) {
                    // when key reach 0xFFFFFFFF (-1) clear cache so we don't put a wrong wrappedObject in there.
                    cacheLock->EnterWriteLock();
                    try {
                        $->Clear();
                        map4cache.clear();
                    }
                    finally {
                        cacheLock->ExitWriteLock();
                    }
                }
                $[obj] = k;
                map4cache[k] = javascriptWrapper;
                return javascriptWrapper;
            }

            bool BindingHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
            {
                auto unmanagedWrapper = static_cast<UnmanagedWrapper*>(object->GetUserData().get());
                Object^ self = unmanagedWrapper->Get();

                if (self == nullptr)
                {
                    exception = "Binding's CLR object is null.";
                    return true;
                }

                String^ methodName = toClr(name);
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                methodName = unmanagedWrapper->GetMethodMapping(methodName);
#endif
                Type^ type = self->GetType();
                auto methods = type->GetMember(methodName, MemberTypes::Method, BindingFlags::Instance | BindingFlags::Public);

                if (methods->Length == 0)
                {
                    exception = toNative("No method named " + methodName + ".");
                    return true;
                }

                //TODO: cache for type info here

                auto suppliedArguments = gcnew array<Object^>(arguments.size());
                try
                {
                    for (int i = 0; i < suppliedArguments->Length; i++) 
                    {
                        suppliedArguments[i] = convertFromCef(arguments[i]);
                    }
                }
                catch (System::Exception^ err)
                {
                    exception = toNative(err->Message);
                    return true;
                }

                MethodInfo^ bestMethod;
                array<Object^>^ bestMethodArguments;

                FindBestMethod(methods, suppliedArguments, bestMethod, bestMethodArguments);

                if (bestMethod != nullptr)
                {
                    try
                    {
                        Object^ result = bestMethod->Invoke(self, bestMethodArguments);
                        retval = convertToCef(result, bestMethod->ReturnType, object);
                        return true;
                    }
                    catch (System::Reflection::TargetInvocationException^ err)
                    {
                        exception = toNative(err->InnerException->Message);
                    }
                    catch (System::Exception^ err)
                    {
                        exception = toNative(err->Message);
                    }
                }
                else
                {
                    exception = toNative("Argument mismatch for method \"" + methodName + "\".");
                }
                return true;
            }

            bool BindingHandler::IsNullableType(Type^ type)
            {
                // This is traditionally checked by this C# code:
                // return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
                // But we have some problems with Nullable<>::typeid.
                return Nullable::GetUnderlyingType(type) != nullptr;
            }

            /// <summary>Gets the cost of changing the type an object to another type.</summary>
            /// <param name="conversionType">The target type.</param>
            /// <return>The conversion cost, or -1 if no conversion available. Lower cost is better.</return>
            int BindingHandler::GetChangeTypeCost(Object^ value, Type^ conversionType)
            {
                // TODO: temporary Int64 support fully disabled
                if (conversionType == Int64::typeid ||
                    conversionType == Nullable<Int64>::typeid ||
                    conversionType == UInt64::typeid ||
                    conversionType == Nullable<UInt64>::typeid)
                {
                    return -1;
                }

                // Actual conversion cost is 0, but set to 2 to give priority to better matches
                if (conversionType == Object::typeid)
                {
                    return 2;
                }

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

                Type^ valueType = value->GetType();

                if (valueType == conversionType) return 0;

                int baseCost = 0;

                // The target type may be nullable, in which case the cost is increased slightly.
                Type^ targetType = Nullable::GetUnderlyingType(conversionType);
                if (targetType != nullptr)
                {
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
                else if (valueType->IsArray && (conversionType->IsArray || conversionType == Object::typeid))
                    return baseCost + 0;
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

                // Converting to Object, so nothing needs to be done.
                if (conversionType == Object::typeid)
                    return value;

                Type^ targetType = Nullable::GetUnderlyingType(conversionType);
                if (targetType != nullptr) conversionType = targetType;

                Type^ inType = value->GetType();
                if (inType == conversionType) return value;

                if (inType->IsArray) {
                    if (!conversionType->HasElementType) return value;

                    Type^ outType = conversionType->GetElementType();
                    if (outType == inType->GetElementType()) return value;

                    Array^ in = (Array^) value;
                    Array^ out = Array::CreateInstance(outType,  in->Length);

                    for (int i = 0; i < in->Length; i++)
                        out->SetValue(Convert::ChangeType(in->GetValue(i), outType), i);

                    return out;
                }
                return Convert::ChangeType(value, conversionType);
            }

            HashSet<String^>^ BindingHandler::GetMethodNames(Type^ type)
            {
                auto methods = type->GetMethods(BindingFlags::Instance | BindingFlags::Public);
                auto result = gcnew HashSet<String^>();

                for each(auto method in methods) 
                {
                    // "Special name"-methods are things like property getters and setters, which we don't want to include in the list.
                    if (method->IsSpecialName) continue;

                    result->Add(method->Name);
                }

                return result;
            }

            void BindingHandler::FindBestMethod(array<MemberInfo^>^ methods, array<Object^>^ suppliedArguments, MethodInfo^% bestMethod, array<Object^>^% bestMethodArguments)
            {
                auto bestMethodCost = -1;

                for (int i = 0; i < methods->Length; i++)
                {
                    auto method = (MethodInfo^)methods[i];
                    auto parametersInfo = method->GetParameters();

                    if (parametersInfo->Length == 0)
                    {
                        if (suppliedArguments->Length == 0)
                        {
                            bestMethod = method;
                            bestMethodArguments = suppliedArguments;
                            bestMethodCost = 0;
                            break;
                        }
                    }
                    else
                    {
                        int failed = 0;
                        int cost = 0;
                        auto arguments = gcnew List<Object^>();

                        try
                        {
                            int p, a;
                            for (p = 0, a = 0; !failed && p < parametersInfo->Length && a < suppliedArguments->Length; p++)
                            {
                                ParameterInfo^ pi = parametersInfo[p];
                                Type^ paramType = pi->ParameterType;

                                if (paramType->IsArray && paramType->GetElementType() == Object::typeid)
                                {
                                    if (p < parametersInfo->Length - 1)
                                    {
                                        failed++;
                                        break;
                                    }

                                    // add remaining arguments as array
                                    auto parm = gcnew List<Object^>();
                                    for (; a < suppliedArguments->Length; a++)
                                        parm->Add(suppliedArguments[a]);
                                    arguments->Add(parm->ToArray());
                                    cost += arguments->Count * 2;
                                }
                                else 
                                {
                                    int paramCost = GetChangeTypeCost(suppliedArguments[a], paramType);
                                    if (paramCost < 0)
                                    {
                                        failed++;
                                        break;
                                    }
                                    arguments->Add(ChangeType(suppliedArguments[a++], paramType));
                                    cost += paramCost;
                                }
                            }

                            // check all required parameters are supplied
                            for (; !failed && p < parametersInfo->Length; p++)
                            {
                                ParameterInfo^ pi = parametersInfo[p];
                                if (pi->IsOptional)
                                {
                                    if (pi->DefaultValue != DBNull::Value)
                                        arguments->Add(pi->DefaultValue);
                                    else
                                        arguments->Add(nullptr);
                                }
                                else if (p == parametersInfo->Length - 1 && pi->ParameterType->IsArray && pi->ParameterType->GetElementType() == Object::typeid)
                                    arguments->Add(gcnew array<Object^>(0));
                                else
                                    failed++;
                            }

                            // check all supplied arguments used
                            if (!failed && a < suppliedArguments->Length)
                                failed++;
                        }
                        catch(Exception^)
                        {
                            failed++;
                        }

                        if (failed > 0)
                            continue;

                        if (cost < bestMethodCost || bestMethodCost < 0)
                        {
                            bestMethod = method;
                            bestMethodArguments = arguments->ToArray();
                            bestMethodCost = cost;
                        }

                        // this is best as possible cost
                        if (cost == 0)
                            break;
                    }
                }
            }

            Dictionary<String^, PropertyInfo^>^ BindingHandler::GetProperties(Type^ type)
            {
                auto properties = type->GetProperties(BindingFlags::Instance | BindingFlags::Public);
                auto result = gcnew Dictionary<String^, PropertyInfo^>();

                for each(auto property in properties)
                {
                    result[property->Name] = property;
                }

                return result;
            }

            void BindingHandler::CreateJavascriptMethods(CefV8Handler* handler, CefRefPtr<CefV8Value> javascriptObject, IEnumerable<String^>^ methodNames)
            {
                for each(String^ methodName in methodNames)
                {
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                    auto jsMethodName = LowercaseFirst(methodName);
                    auto unmanagedWrapper = static_cast<UnmanagedWrapper*>(javascriptObject->GetUserData().get());
                    if (!unmanagedWrapper->AddMethodMapping(methodName, jsMethodName)) continue;
#else
                    auto jsMethodName = methodName;
#endif
                    auto nameStr = toNative(jsMethodName);
                    auto fun = CefV8Value::CreateFunction(nameStr, handler);
                    javascriptObject->SetValue(nameStr, fun, V8_PROPERTY_ATTRIBUTE_NONE);
                    fun->AddRef();
                }
            }

            void BindingHandler::CreateJavascriptProperties(CefRefPtr<CefV8Value> javascriptObject, Dictionary<String^, PropertyInfo^>^ properties)
            {

                for each(String^ propertyName in properties->Keys)
                {
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                    auto jsPropertyName = LowercaseFirst(propertyName);
                    auto unmanagedWrapper = static_cast<UnmanagedWrapper*>(javascriptObject->GetUserData().get());
                    if (!unmanagedWrapper->AddPropertyMapping(propertyName, jsPropertyName)) continue;
#else
                    auto jsPropertyName = propertyName;
#endif
                    auto nameStr = toNative(jsPropertyName);

                    auto ctrl = V8_ACCESS_CONTROL_DEFAULT;
                    auto propertyAttribute = V8_PROPERTY_ATTRIBUTE_NONE;
                    auto prop = properties[propertyName];

                    auto get = prop->GetGetMethod();
                    if (get != nullptr && get->IsPublic)
                    {
                        ctrl = static_cast<CefV8Value::AccessControl>(ctrl | V8_ACCESS_CONTROL_ALL_CAN_READ);
                    }

                    auto set = prop->GetSetMethod();
                    if (set != nullptr && set->IsPublic)
                        ctrl = static_cast<CefV8Value::AccessControl>(ctrl | V8_ACCESS_CONTROL_ALL_CAN_WRITE);
                    else
                    {
                         ctrl = static_cast<CefV8Value::AccessControl>(ctrl | V8_ACCESS_CONTROL_PROHIBITS_OVERWRITING);
                        propertyAttribute = V8_PROPERTY_ATTRIBUTE_READONLY;
                    }

                     javascriptObject->SetValue(nameStr, ctrl, propertyAttribute);
                }
            }

            String^ BindingHandler::LowercaseFirst(String^ str)
            {
                if (str == String::Empty)
                {
                    return str;
                }

                return Char::ToLower(str[0]) + str->Substring(1);
            }
        }
    }
}
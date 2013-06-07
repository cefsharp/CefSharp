#include "stdafx.h"
#include "BindingHandler.h"

using namespace System::ComponentModel;

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
		else if (valueType->IsArray && (conversionType->IsArray || conversionType == Object::typeid))
			return baseCost + 0;
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

        array<Object^>^ suppliedArguments = gcnew array<Object^>(arguments.size());
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
                retval = convertToCef(result, bestMethod->ReturnType, object);
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

	CefRefPtr<CefV8Value> BindingHandler::Bind(Object^ obj, CefRefPtr<CefV8Value> window)
    {
		IDictionary<Object^, unsigned long>^ $ = cache;
		{
			//lock x(cache);
			cacheLock->EnterReadLock();
			try {
				if (cache->ContainsKey(obj)) 
					return map4cache[$[obj]];
			}
			finally {
				cacheLock->ExitReadLock();
			}
		}

        //CefRefPtr<BindingData> bindingData = new BindingData(obj);
        CefRefPtr<CefBase> userData = new BindingData(obj);
        static CefRefPtr<CefV8Handler> handler = new BindingHandler();
		static CefRefPtr<CefV8Accessor> acc = new Accessor();
        CefRefPtr<CefV8Value> wrappedObject = window->CreateObject(acc);
		wrappedObject->SetUserData(userData);
		//wrappedObject->AdjustExternallyAllocatedMemory(100 * 1024 * 1024); // TODO: need to find out size of obj!

		array<MemberInfo^>^ members = obj->GetType()->GetMembers(BindingFlags::Instance | BindingFlags::Public);

        IList<String^>^ methodNames = gcnew List<String^>();
		for each(MemberInfo^ mi in members) 
		{
			CefString nameStr = toNative(mi->Name);
			
			// filter out certain system type properties               
			if (mi->DeclaringType == System::Dynamic::DynamicObject::typeid || mi->DeclaringType == System::MarshalByRefObject::typeid || mi->DeclaringType == System::Object::typeid) continue;

			if (mi->DeclaringType->GetInterface(System::Reflection::IReflect::typeid->ToString()) != nullptr) {
				bool browsable = true;
				for each(Object^ o in mi->GetCustomAttributes(EditorBrowsableAttribute::typeid, false)) {
					EditorBrowsableAttribute^ a = (EditorBrowsableAttribute^) o;
					if (a->State ==  EditorBrowsableState::Never) {
						browsable = false;
						break;
					}
				}
				if (!browsable) continue;
			}

			if (mi->MemberType == MemberTypes::Method) {
				if(methodNames->Contains(mi->Name)) continue;
				methodNames->Add(mi->Name);
				MethodInfo^ m = (MethodInfo^) mi;
				if (!m->IsSpecialName && !m->IsConstructor && m->GetBaseDefinition()->DeclaringType !=  System::Dynamic::DynamicObject::typeid)
					wrappedObject->SetValue(nameStr, CefV8Value::CreateFunction(nameStr, handler), V8_PROPERTY_ATTRIBUTE_NONE);
			}
			else if (mi->MemberType == MemberTypes::Property) {
				PropertyInfo^ p = (PropertyInfo^) mi;
				if (!p->IsSpecialName && (p->DeclaringType == nullptr || 
					( (p->DeclaringType->Attributes & TypeAttributes::SpecialName) != TypeAttributes::SpecialName && !p->IsSpecialName && p->GetIndexParameters()->Length == 0))) {
					CefV8Value::AccessControl ctrl = V8_ACCESS_CONTROL_DEFAULT;
					if (p->CanRead)
						ctrl = static_cast<CefV8Value::AccessControl>(ctrl | V8_ACCESS_CONTROL_ALL_CAN_READ);
					if (p->CanWrite)
						ctrl = static_cast<CefV8Value::AccessControl>(ctrl | V8_ACCESS_CONTROL_ALL_CAN_WRITE);
					wrappedObject->SetValue(nameStr, ctrl, V8_PROPERTY_ATTRIBUTE_NONE);
				}
			}
		}

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
		map4cache[k] = wrappedObject;
        return wrappedObject;
    }

    void BindingHandler::Bind(String^ name, Object^ obj, CefRefPtr<CefV8Value> window)
    {
        window->SetValue(toNative(name), Bind(obj, window), V8_PROPERTY_ATTRIBUTE_NONE);
    }

	bool Accessor::Get(const CefString& name,
									const CefRefPtr<CefV8Value> object,
									CefRefPtr<CefV8Value>& retval,
									CefString& exception) {
			CefRefPtr<BindingData> bindingData = static_cast<BindingData*>(object->GetUserData().get());
			Object^ self = bindingData->Get();
			if(self == nullptr) 
			{
				exception = "Binding's CLR object is null.";
				return true;
			}

			String^ memberName = toClr(name);
			Type^ type = self->GetType();
			PropertyInfo^ p = type->GetProperty(memberName, BindingFlags::Instance | BindingFlags::Public);

			if(p == nullptr)
			{
				exception = toNative("No member named " + memberName + ".");
				return true;
			}

			try {
				Object^ result = p->GetValue(self, nullptr);
				retval = result == self ? object : convertToCef(result, p->PropertyType, object);
			}
			catch (Exception^ e) {
				exception = toNative(e->Message);
				return true;
			}

			return true;
	}

      bool  Accessor::Set(const CefString& name,
                       const CefRefPtr<CefV8Value> object,
                       const CefRefPtr<CefV8Value> value,
                       CefString& exception) {						           
	   CefRefPtr<BindingData> bindingData = static_cast<BindingData*>(object->GetUserData().get());
	   Object^ self = bindingData->Get();
	   if(self == nullptr) 
	   {
		   exception = "Binding's CLR object is null.";
		   return true;
	   }

	   String^ memberName = toClr(name);
	   Type^ type = self->GetType();
	   PropertyInfo^ p = type->GetProperty(memberName, BindingFlags::Instance | BindingFlags::Public);

	   if(p == nullptr)
	   {
		   exception = toNative("No member named " + memberName + ".");
		   return true;
	   }

	   Object^ v = value == object ? self : convertFromCef(value);	   
	   p->SetValue(self, BindingHandler::ChangeType(v, p->PropertyType), nullptr);

        return true;
      }
}
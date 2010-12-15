#include "stdafx.h"
#include "BindingHandler.h"

namespace CefSharp
{

    CefRefPtr<CefV8Value> BindingHandler::ConvertToCef(Type^ type, Object^ obj)
    {
        if(type == Void::typeid)
        {
            return CefV8Value::CreateUndefined();
        }
        if(obj == nullptr)
        {
            return CefV8Value::CreateNull();
        }

	    if (type == Boolean::typeid)
        {
            return CefV8Value::CreateBool(safe_cast<bool>(obj));
        }
	    if (type == Int16::typeid || type == Int32::typeid)
        {
            return CefV8Value::CreateInt(safe_cast<int>(obj));
        }
	    if (type == Single::typeid || type == Double::typeid)
        {
            return CefV8Value::CreateDouble(safe_cast<double>(obj));
        }
        if (type == String::typeid)
	    {
            CefString str = convertFromString(safe_cast<String^>(obj));
            return CefV8Value::CreateString(str);
	    }
        
        throw gcnew Exception("cannot convert object from CLR to Cef " + type->ToString());
    }

    Object^ BindingHandler::ConvertFromCef(CefRefPtr<CefV8Value> obj)
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
            return convertToString(obj->GetStringValue());
        }
        
        throw gcnew Exception("cannot convert object from Cef to CLR");
    }

    bool BindingHandler::Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
    {
        CefRefPtr<BindingData> bindingData = reinterpret_cast<BindingData*>(object->GetUserData().get());
        Object^ self = bindingData->Get();
        if(self == nullptr) 
        {
            exception = "binding's CLR object is null";
            return true;
        }

        String^ memberName = convertToString(name);
        Type^ type = self->GetType();
        array<System::Reflection::MemberInfo^>^ members = type->GetMember(memberName, MemberTypes::Method, 
            BindingFlags::IgnoreCase | BindingFlags::Instance | BindingFlags::Public);

        if(members->Length == 0)
        {
            exception = convertFromString("no member named " + memberName);
            return true;
        }

        array<System::Object^>^ suppliedArguments = gcnew array<Object^>(arguments.size());
        for(int i = 0; i < suppliedArguments->Length; i++) 
        {
            try
            {
                suppliedArguments[i] = ConvertFromCef(arguments[i]);
            }
            catch(System::Exception^ err)
            {
                exception = convertFromString(err->Message);
                return true;
            }
        }

        MethodInfo^ bestMethod;
        array<Object^>^ bestMethodArguments;
        int bestMethodMatchedArgs = -1;

        for (int i = 0; i < members->Length; i++)
	    {
		    MethodInfo^ method = (MethodInfo^) members[i];
		    array<ParameterInfo^>^ parametersInfo = method->GetParameters();
		    array<Object^>^ arguments;
            
		    if (suppliedArguments->Length == parametersInfo->Length)
		    {
			    int match = 0;
			    int failed = 0;

			    
			    arguments = gcnew array<Object^>(suppliedArguments->Length);
			    for (int p = 0; p < suppliedArguments->Length; p++)
			    {
				    System::Type^ paramType = parametersInfo[p]->ParameterType;

				    if (suppliedArguments[p] != nullptr)
				    {
					    System::Type^ suppliedType = suppliedArguments[p]->GetType();

					    if (suppliedType == paramType)
					    {
						    arguments[p] = suppliedArguments[p];
						    match++;
					    }
					    else
					    {
                            // TODO
						    //arguments[p] = ConvertToType(suppliedArguments[p], paramType);
						    //if (arguments[p] == nullptr)
						    //{
							    failed++;
							    break;
						    //}
					    }
				    }
			    }

			    if (failed > 0)
                {
				    continue;
                }

			    if (match > bestMethodMatchedArgs)
			    {
				    bestMethod = method;
				    bestMethodArguments = arguments;
				    bestMethodMatchedArgs = match;
			    }

			    if (match == arguments->Length)
				    break;
		    }
        }

        if (bestMethod != nullptr)
        {
            try
            {
	            Object^ result = bestMethod->Invoke(self, bestMethodArguments);
                retval = ConvertToCef(bestMethod->ReturnType, result);
                return true;
            }
            catch(System::Reflection::TargetInvocationException^ err)
            {
                exception = convertFromString(err->InnerException->Message);
            }
            catch(System::Exception^ err)
            {
                exception = convertFromString(err->Message);
            }
        }
        else
        {
            exception = convertFromString("Argument mismatch for method \"" + memberName + "\".");
        }
        return true;
    }

    void BindingHandler::Bind(String^ name, Object^ obj, CefRefPtr<CefV8Value> window)
    {
        CefRefPtr<BindingData> bindingData = new BindingData(obj);
        CefRefPtr<CefBase> userData = static_cast<CefRefPtr<CefBase>>(bindingData);
        CefRefPtr<CefV8Value> wrappedObject = window->CreateObject(userData);
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
            CefString nameStr = convertFromString(methodName);
            wrappedObject->SetValue(nameStr, CefV8Value::CreateFunction(nameStr, handler));
        }

        window->SetValue(convertFromString(name), wrappedObject);
    }

}
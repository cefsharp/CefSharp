#include "stdafx.h"
#include <msclr\lock.h>
#pragma once

using namespace System::Reflection;
using namespace System::Collections::Generic;
using namespace System::Collections::Concurrent;
using namespace msclr;
using namespace System::Threading;

namespace CefSharp
{
	static gcroot<IDictionary<Object^, unsigned long>^> cache = gcnew Dictionary<Object^, unsigned long>();
	static std::map<unsigned long,  CefRefPtr<CefV8Value>> map4cache;
	static gcroot<ReaderWriterLockSlim^> cacheLock = gcnew ReaderWriterLockSlim();

    class BindingData : public CefBase
    {
    protected:
        gcroot<Object^> _obj;

    public:
        BindingData(Object^ obj)
        {
            _obj = obj;
        }

		virtual ~BindingData() 
		{			
			//System::Console::WriteLine("Destroying binding object " + _obj->GetType()->ToString());
			//lock x(cache);
			if (cache->ContainsKey(_obj)) {
				cacheLock->EnterWriteLock();
				try {
					IDictionary<Object^, unsigned long>^ $ = cache;
					map4cache.erase($[_obj]);
					cache->Remove(_obj);
				}
				finally {
					cacheLock->ExitWriteLock();
				}
			}
			//_obj = nullptr;
		}

        Object^ Get()
        {
            return _obj;
        }

        IMPLEMENT_REFCOUNTING(BindingData);
    };

	class Accessor : public CefV8Accessor {
     public:
      virtual bool Get(const CefString& name,
                       const CefRefPtr<CefV8Value> object,
                       CefRefPtr<CefV8Value>& retval,
                       CefString& exception);

      virtual bool Set(const CefString& name,
                       const CefRefPtr<CefV8Value> object,
                       const CefRefPtr<CefV8Value> value,
                       CefString& exception);

      IMPLEMENT_REFCOUNTING(Accessor);
    };

    class BindingHandler : public CefV8Handler
    {
        // Type Converter
        static bool IsNullableType(Type^ type);
        static int GetChangeTypeCost(Object^ value, Type^ conversionType);
 
        CefRefPtr<CefV8Value> ConvertToCef(Object^ obj, Type^ type);
        Object^ ConvertFromCef(CefRefPtr<CefV8Value> obj);
        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);
    public:
        static Object^ ChangeType(Object^ value, Type^ conversionType);
		static CefRefPtr<CefV8Value> Bind(Object^ obj, CefRefPtr<CefV8Value> window);
        static void Bind(String^ name, Object^ obj, CefRefPtr<CefV8Value> window);

        IMPLEMENT_REFCOUNTING(BindingHandler);
    };
}
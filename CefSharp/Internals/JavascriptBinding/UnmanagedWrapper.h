#include "Stdafx.h"
#pragma once
#define CHANGE_FIRST_CHAR_TO_LOWER // comment out this if want to keep the orignal case of methods and properties

using namespace System::Collections::Generic;
using namespace System::Reflection;
using namespace System::Collections::Concurrent;
using namespace System::Threading;

namespace CefSharp
{
	namespace Internals
	{
		namespace JavascriptBinding
		{
            static gcroot<IDictionary<Object^, unsigned long>^> cache = gcnew Dictionary<Object^, unsigned long>();
            static std::map<unsigned long, CefRefPtr<CefV8Value>> map4cache;
            static gcroot<ReaderWriterLockSlim^> cacheLock = gcnew ReaderWriterLockSlim();
			/// <summary>
			/// Acts as an unmanaged wrapper for a managed .NET object.
			/// </summary>
			private class UnmanagedWrapper : public CefBase
			{
			protected:
				gcroot<Object^> _obj;
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                gcroot<Dictionary<String^, String^>^> _methodMap;
                gcroot<Dictionary<String^, String^>^> _propertyMap;
#endif
			public:

				UnmanagedWrapper(Object^ obj)
				{
					_obj = obj;
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                    _methodMap = gcnew Dictionary<String^, String^>();
                    _propertyMap = gcnew Dictionary<String^, String^>();
#endif
				}

                virtual ~UnmanagedWrapper()
                {
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
                }

				/// <summary>
				/// Gets a reference to the wrapped object.
				/// </summary>
				/// <returns>The wrapped object.</returns>
				Object^ Get()
				{
					return _obj;
				}

#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                bool AddMethodMapping(String^ from, String^ to)
                {
                    if (_methodMap->ContainsKey(to) && Char::IsLower(from[0])) return false; // another method already exists!
                    ((IDictionary<String^, String^>^) _methodMap)[to] = from;
                    return true;
                }

                String^ GetMethodMapping(String^ from)
                {
                    String^ value;

                    if (_methodMap->TryGetValue(from, value))
                    {
                        return value;
                    }
                    else
                    {
                        return nullptr;
                    }
                }

                bool AddPropertyMapping(String^ from, String^ to)
                {
                    if (_propertyMap->ContainsKey(to) && Char::IsLower(from[0])) return false; // another property already exists!
                    ((IDictionary<String^, String^>^) _propertyMap)[to] = from;
                    return true;
                }

                String^ GetPropertyMapping(String^ from)
                {
                    String^ value;

                    if (_propertyMap->TryGetValue(from, value))
                    {
                        return value;
                    }
                    else
                    {
                        return nullptr;
                    }
                }
#endif
				IMPLEMENT_REFCOUNTING(UnmanagedWrapper);
            };
		}
	}
}
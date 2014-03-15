#include "Stdafx.h"
#pragma once

using namespace System::Collections::Generic;
using namespace System::Reflection;

namespace CefSharp
{
	namespace Internals
	{
		namespace JavascriptBinding
		{
			/// <summary>
			/// Acts as an unmanaged wrapper for a managed .NET object.
			/// </summary>
			private class UnmanagedWrapper : public CefBase
			{
			protected:
				gcroot<Object^> _obj;
                gcroot<Dictionary<String^, String^>^> _methodMap;
                gcroot<Dictionary<String^, String^>^> _propertyMap;

			public:
				gcroot<Dictionary<String^, PropertyInfo^>^> Properties;

				UnmanagedWrapper(Object^ obj)
				{
					_obj = obj;
                    _methodMap = gcnew Dictionary<String^, String^>();
                    _propertyMap = gcnew Dictionary<String^, String^>();
				}

				/// <summary>
				/// Gets a reference to the wrapped object.
				/// </summary>
				/// <returns>The wrapped object.</returns>
				Object^ Get()
				{
					return _obj;
				}

                void AddMethodMapping(String^ from, String^ to)
                {
                    _methodMap->Add(to, from);
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

                void AddPropertyMapping(String^ from, String^ to)
                {
                    _propertyMap->Add(to, from);
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

				IMPLEMENT_REFCOUNTING(UnmanagedWrapper);
            };
		}
	}
}
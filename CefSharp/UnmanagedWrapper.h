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

			public:
				gcroot<Dictionary<String^, PropertyInfo^>^> Properties;

				UnmanagedWrapper(Object^ obj)
				{
					_obj = obj;
				}

				/// <summary>
				/// Gets a reference to the wrapped object.
				/// </summary>
				/// <returns>The wrapped object.</returns>
				Object^ Get()
				{
					return _obj;
				}

				IMPLEMENT_REFCOUNTING(UnmanagedWrapper);
			};
		}
	}
}
#include "Stdafx.h"
#pragma once

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
	        static std::map<unsigned long,  CefRefPtr<CefV8Value>> map4cache;
	        static gcroot<ReaderWriterLockSlim^> cacheLock = gcnew ReaderWriterLockSlim();
			/// <summary>
			/// Acts as an unmanaged wrapper for a managed .NET object.
			/// </summary>
			private class UnmanagedWrapper : public CefBase
			{
			protected:
				gcroot<Object^> _obj;

			public:
				UnmanagedWrapper(Object^ obj)
				{
					_obj = obj;
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

				IMPLEMENT_REFCOUNTING(UnmanagedWrapper);
            };
		}
	}
}
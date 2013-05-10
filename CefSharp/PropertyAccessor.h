#include "Stdafx.h"

namespace CefSharp
{
	namespace Internals
	{
		namespace JavascriptBinding
		{
			private class PropertyAccessor : CefV8Accessor
			{
				///
				// Handle retrieval the accessor value identified by |name|. |object| is the
				// receiver ('this' object) of the accessor. If retrieval succeeds set
				// |retval| to the return value. If retrieval fails set |exception| to the
				// exception that will be thrown. Return true if accessor retrieval was
				// handled.
				///
				/*--cef()--*/
				virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
								 CefString& exception) override
				{
					return false;
				}

				///
				// Handle assignment of the accessor value identified by |name|. |object| is
				// the receiver ('this' object) of the accessor. |value| is the new value
				// being assigned to the accessor. If assignment fails set |exception| to the
				// exception that will be thrown. Return true if accessor assignment was
				// handled.
				///
				/*--cef()--*/
				virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
								 CefString& exception) override
				{
					return false;
				}

				IMPLEMENT_REFCOUNTING(PropertyAccessor)
			};
		}
	}
}
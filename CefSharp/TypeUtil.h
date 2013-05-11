#include "Stdafx.h"
#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
	/// <summary>
	///	Converts a .NET object to an (unmanaged) Chromium V8 object.
	/// </summary>
	/// <param name="obj">The .NET object that should be converted.</param>
	/// <param name="type">The type of the source object. If this parameter is a null reference, the type will be determined
	/// automatically.</param>
	/// <returns>A corresponding V8 value.</returns>
    CefRefPtr<CefV8Value> convertToCef(Object^ obj, Type^ type);

	/// <summary>
	///	Converts a Chromium V8 value to a (managed) .NET object.
	/// </summary>
	/// <param name="obj">The V8 value that should be converted.</param>
	/// <returns>A corresponding .NET object.</returns>
	Object^ convertFromCef(CefRefPtr<CefV8Value> obj);
}
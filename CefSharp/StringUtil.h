#include "Stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
	/// <summary>
	///	Converts an unmanaged string to a (managed) .NET string.
	/// </summary>
	/// <param name="cefStr">The string that should be converted.</param>
	/// <returns>A .NET string.</returns>
    String^ toClr(const cef_string_t& cefStr);

	/// <summary>
	///	Converts an unmanaged string to a (managed) .NET string.
	/// </summary>
	/// <param name="cefStr">The string that should be converted.</param>
	/// <returns>A .NET string.</returns>
	String^ toClr(const CefString& cefStr);

	/// <summary>
	///	Converts a .NET string to native (unamanged) format. Note that this method does not allocate a new copy of the string, but
	/// rather returns a pointer to the memory in the existing managed String object.
	/// </summary>
	/// <param name="str">The string that should be converted.</param>
	/// <returns>An unmanaged representation of the provided string.</returns>
    CefString toNative(String^ str);
    
	void assignFromString(cef_string_t& cefStrT, String^ str);
}
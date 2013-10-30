// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "include/cef_app.h"

namespace CefSharp
{
	namespace Wrappers
	{
		private class CefAppUnmanagedWrapper : CefApp
		{
			IMPLEMENT_REFCOUNTING(CefAppUnmanagedWrapper);
		};

		// TODO: Support the stuff that CefApp has. One important question is whether we go with the C++ API or make the (seemingly a bit
		// strange) move to start using the C API instead. It may actually make it possible to write our code in a cleaner fashion. Worth
		// thinking about.
		public ref class CefAppWrapper
		{
		private:
			CefRefPtr<CefAppUnmanagedWrapper>* cefApp;
			bool is_disposed;

		public:
			CefAppWrapper();
			~CefAppWrapper();
			void* GetUnmanaged();

		private:
			!CefAppWrapper();
		};
	}
}

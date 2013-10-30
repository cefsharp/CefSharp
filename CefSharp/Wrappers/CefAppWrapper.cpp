// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefAppWrapper.h"

namespace CefSharp
{
	namespace Wrappers
	{
		CefAppWrapper::CefAppWrapper()
		{
			cefApp = new CefRefPtr<CefAppUnmanagedWrapper>(new CefAppUnmanagedWrapper());
		}

		CefAppWrapper::~CefAppWrapper()
		{
			if (is_disposed) return;
			this->!CefAppWrapper();
			is_disposed = true;
		}

		void* CefAppWrapper::GetUnmanaged()
		{
			return cefApp;
		}

		CefAppWrapper::!CefAppWrapper()
		{
			delete cefApp;
		}
	};
}

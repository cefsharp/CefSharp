// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "SchemeHandlerFactoryWrapper.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
	CefRefPtr<CefResourceHandler> SchemeHandlerFactoryWrapper::Create(
		CefRefPtr<CefBrowser> browser,
		CefRefPtr<CefFrame> frame,
		const CefString& scheme_name,
		CefRefPtr<CefRequest> request)
	{
		ISchemeHandler^ handler = _factory->Create();
		CefRefPtr<SchemeHandlerWrapper> wrapper = new SchemeHandlerWrapper(handler);
		return static_cast<CefRefPtr<CefResourceHandler>>(wrapper);
	}
}
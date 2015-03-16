// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_scheme.h"
#include "Internals/AutoLock.h"

using namespace System;
using namespace System::IO;
using namespace System::Collections::Specialized;

namespace CefSharp
{
	class SchemeHandlerFactoryWrapper : public CefSchemeHandlerFactory
	{
		gcroot<ISchemeHandlerFactory^> _factory;

	public:
		SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory)
			: _factory(factory) {}

		virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& scheme_name, CefRefPtr<CefRequest> request);

		IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
	};
}
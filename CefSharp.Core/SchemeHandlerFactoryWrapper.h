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
	private class SchemeHandlerFactoryWrapper : public CefSchemeHandlerFactory
	{
		gcroot<ISchemeHandlerFactory^> _factory;

	public:
		SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory)
			: _factory(factory) {}

		~SchemeHandlerFactoryWrapper()
		{
			_factory = nullptr;
		}

		virtual CefRefPtr<CefResourceHandler> SchemeHandlerFactoryWrapper::Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& scheme_name, CefRefPtr<CefRequest> request) OVERRIDE
		{
			auto handler = _factory->Create();
			CefRefPtr<ResourceHandlerWrapper> wrapper = new ResourceHandlerWrapper(handler);
			return static_cast<CefRefPtr<CefResourceHandler>>(wrapper);
		}

		IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
	};
}
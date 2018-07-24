// Copyright � 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request_context.h""
#include "include\cef_request_context_handler.h"

namespace CefSharp
{
	namespace Internals
	{
		private class CefRequestContextHandlerAdapter : public CefRequestContextHandler
		{
			gcroot<IRequestContextHandler^> _requestContextHandler;

		public:
			CefRequestContextHandlerAdapter(IRequestContextHandler^ requestContextHandler)
				: _requestContextHandler(requestContextHandler)
			{
			}

			~CefRequestContextHandlerAdapter()
			{
				_requestContextHandler = nullptr;
			}

			virtual CefRefPtr<CefCookieManager> GetCookieManager() OVERRIDE;

			virtual bool OnBeforePluginLoad(const CefString& mime_type,
				const CefString& plugin_url,
				bool is_main_frame,
				const CefString& top_origin_url,
				CefRefPtr<CefWebPluginInfo> plugin_info,
				CefRequestContextHandler::PluginPolicy* plugin_policy) OVERRIDE;

			virtual void OnRequestContextInitialized(CefRefPtr<CefRequestContext> requestContext) OVERRIDE;

			IMPLEMENT_REFCOUNTING(CefRequestContextHandlerAdapter);
		};
	}
}
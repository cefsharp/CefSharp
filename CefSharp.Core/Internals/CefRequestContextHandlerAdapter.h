// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_INTERNALS_CEFREQUESTCONTEXTHANDLERADAPTER_H_
#define CEFSHARP_CORE_INTERNALS_CEFREQUESTCONTEXTHANDLERADAPTER_H_

#pragma once

#include "Stdafx.h"

#include "include\cef_request_context.h"
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

            virtual bool OnBeforePluginLoad(const CefString& mime_type,
                const CefString& plugin_url,
                bool is_main_frame,
                const CefString& top_origin_url,
                CefRefPtr<CefWebPluginInfo> plugin_info,
                CefRequestContextHandler::PluginPolicy* plugin_policy) OVERRIDE;

            virtual void OnRequestContextInitialized(CefRefPtr<CefRequestContext> requestContext) OVERRIDE;

            virtual CefRefPtr<CefResourceRequestHandler> GetResourceRequestHandler(
                CefRefPtr<CefBrowser> browser,
                CefRefPtr<CefFrame> frame,
                CefRefPtr<CefRequest> request,
                bool is_navigation,
                bool is_download,
                const CefString& request_initiator,
                bool& disable_default_handling) OVERRIDE;

            IMPLEMENT_REFCOUNTING(CefRequestContextHandlerAdapter);
        };
    }
}
#endif  // CEFSHARP_CORE_INTERNALS_CEFREQUESTCONTEXTHANDLERADAPTER_H_

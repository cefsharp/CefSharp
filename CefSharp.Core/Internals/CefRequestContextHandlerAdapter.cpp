// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefRequestContextHandlerAdapter.h"
#include "CookieManager.h"
#include "Request.h"
#include "RequestContext.h"
#include "Internals\ReportUnhandledExceptions.h"
#include "Internals\TypeConversion.h"
#include "Internals\CefBrowserWrapper.h"
#include "Internals\CefFrameWrapper.h"
#include "Internals\CefResourceRequestHandlerAdapter.h"

using namespace CefSharp::Core;

namespace CefSharp
{
    namespace Internals
    {
        bool CefRequestContextHandlerAdapter::OnBeforePluginLoad(const CefString& mime_type,
            const CefString& plugin_url,
            bool is_main_frame,
            const CefString& top_origin_url,
            CefRefPtr<CefWebPluginInfo> plugin_info,
            CefRequestContextHandler::PluginPolicy* plugin_policy)
        {
            if (Object::ReferenceEquals(_requestContextHandler, nullptr))
            {
                return false;
            }

            auto pluginInfo = TypeConversion::FromNative(plugin_info);
            auto pluginPolicy = (CefSharp::PluginPolicy)*plugin_policy;

            auto result = _requestContextHandler->OnBeforePluginLoad(StringUtils::ToClr(mime_type),
                StringUtils::ToClr(plugin_url),
                is_main_frame,
                StringUtils::ToClr(top_origin_url),
                pluginInfo,
                pluginPolicy);

            *plugin_policy = (CefRequestContextHandler::PluginPolicy)pluginPolicy;

            return result;
        }

        void CefRequestContextHandlerAdapter::OnRequestContextInitialized(CefRefPtr<CefRequestContext> requestContext)
        {
            if (!Object::ReferenceEquals(_requestContextHandler, nullptr))
            {
                RequestContext ctx(requestContext);

                _requestContextHandler->OnRequestContextInitialized(%ctx);
            }
        }

        CefRefPtr<CefResourceRequestHandler> CefRequestContextHandlerAdapter::GetResourceRequestHandler(
            CefRefPtr<CefBrowser> browser,
            CefRefPtr<CefFrame> frame,
            CefRefPtr<CefRequest> request,
            bool is_navigation,
            bool is_download,
            const CefString& request_initiator,
            bool& disable_default_handling)
        {
            if (Object::ReferenceEquals(_requestContextHandler, nullptr))
            {

                return NULL;
            }

            CefBrowserWrapper browserWrapper(browser);
            CefFrameWrapper frameWrapper(frame);
            Request requestWrapper(request);

            auto handler = _requestContextHandler->GetResourceRequestHandler(%browserWrapper, %frameWrapper, %requestWrapper, is_navigation, is_download, StringUtils::ToClr(request_initiator), disable_default_handling);

            if (Object::ReferenceEquals(handler, nullptr))
            {
                return NULL;
            }

            //CefRequestContext is not associated with a specific browser
            //so browserControl param is always nullptr.
            return new CefResourceRequestHandlerAdapter(nullptr, handler);
        }
    }
}

// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefRequestContextHandlerAdapter.h"
#include "CookieManager.h"
#include "RequestContext.h"
#include "Internals\ReportUnhandledExceptions.h"
#include "Internals\TypeConversion.h"

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
    }
}

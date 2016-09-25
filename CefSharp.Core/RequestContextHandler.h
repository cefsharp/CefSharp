// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "Internals\TypeConversion.h"
#include "Internals\CookieManager.h"

namespace CefSharp
{
    private class RequestContextHandler : public CefRequestContextHandler
    {
        gcroot<IRequestContextHandler^> _requestContextHandler;

    public:
        RequestContextHandler(IRequestContextHandler^ requestContextHandler)
            : _requestContextHandler(requestContextHandler)
        {
        }

        ~RequestContextHandler()
        {
            _requestContextHandler = nullptr;
        }

        virtual CefRefPtr<CefCookieManager> GetCookieManager() OVERRIDE
        {
            if (Object::ReferenceEquals(_requestContextHandler, nullptr))
            {
                return NULL;
            }

            auto cookieManager = _requestContextHandler->GetCookieManager();

            if (cookieManager == nullptr)
            {
                return NULL;
            }

            return (CookieManager^)cookieManager;
        }

        virtual bool OnBeforePluginLoad(const CefString& mime_type,
            const CefString& plugin_url,
            const CefString& top_origin_url,
            CefRefPtr<CefWebPluginInfo> plugin_info,
            CefRequestContextHandler::PluginPolicy* plugin_policy) OVERRIDE
        {
            if (!Object::ReferenceEquals(_requestContextHandler, nullptr))
            {
                auto pluginInfo = TypeConversion::FromNative(plugin_info);
                auto pluginPolicy = (CefSharp::PluginPolicy)*plugin_policy;

                auto result = _requestContextHandler->OnBeforePluginLoad(StringUtils::ToClr(mime_type),
                                                                StringUtils::ToClr(plugin_url),
                                                                StringUtils::ToClr(top_origin_url),
                                                                pluginInfo,
                                                                pluginPolicy);

                *plugin_policy = (CefRequestContextHandler::PluginPolicy)pluginPolicy;

                return result;
            }
            return false;
        }

        IMPLEMENT_REFCOUNTING(RequestContextHandler);
    };
}
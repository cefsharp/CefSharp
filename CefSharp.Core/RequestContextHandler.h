// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "Internals\TypeConversion.h"

namespace CefSharp
{
    private class RequestContextHandler : public CefRequestContextHandler
    {
        gcroot<IPluginHandler^> _pluginHandler;

    public:
        RequestContextHandler(IPluginHandler^ pluginHandler)
            : _pluginHandler(pluginHandler)
        {
        }

        ~RequestContextHandler()
        {
            _pluginHandler = nullptr;
        }

        virtual bool OnBeforePluginLoad(const CefString& mime_type,
            const CefString& plugin_url,
            const CefString& top_origin_url,
            CefRefPtr<CefWebPluginInfo> plugin_info,
            CefRequestContextHandler::PluginPolicy* plugin_policy) OVERRIDE
        {
            if (!Object::ReferenceEquals(_pluginHandler, nullptr))
            {
                auto pluginInfo = TypeConversion::FromNative(plugin_info);
                auto pluginPolicy = (CefSharp::PluginPolicy)*plugin_policy;

                auto result = _pluginHandler->OnBeforePluginLoad(StringUtils::ToClr(mime_type),
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
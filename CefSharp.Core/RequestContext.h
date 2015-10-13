// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_request_context.h"
#include "RequestContextSettings.h"
#include "SchemeHandlerFactoryWrapper.h"
#include "RequestContextHandler.h"

namespace CefSharp
{
    /// <summary>
    /// A request context provides request handling for a set of related browser objects.
    /// A request context is specified when creating a new browser object via the CefBrowserHost
    /// static factory methods. Browser objects with different request contexts will never be
    /// hosted in the same render process. Browser objects with the same request context may or
    /// may not be hosted in the same render process depending on the process model.
    /// Browser objects created indirectly via the JavaScript window.open function or targeted
    /// links will share the same render process and the same request context as the source browser.
    /// When running in single-process mode there is only a single render process (the main process)
    /// and so all browsers created in single-process mode will share the same request context.
    /// This will be the first request context passed into a CefBrowserHost static factory method
    /// and all other request context objects will be ignored. 
    /// </summary>
    public ref class RequestContext
    {
    private:
        MCefRefPtr<CefRequestContext> _requestContext;
        RequestContextSettings^ _settings;
    public:
        RequestContext()
        {
            CefRequestContextSettings settings;
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        RequestContext(RequestContextSettings^ settings) : _settings(settings)
        {
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        RequestContext(IPluginHandler^ pluginHandler)
        {
            CefRequestContextSettings settings;
            _requestContext = CefRequestContext::CreateContext(settings, new RequestContextHandler(pluginHandler));
        }

        RequestContext(RequestContextSettings^ settings, IPluginHandler^ pluginHandler) : _settings(settings)
        {
            _requestContext = CefRequestContext::CreateContext(settings, new RequestContextHandler(pluginHandler));
        }

        !RequestContext()
        {
            _requestContext = NULL;
        }

        ~RequestContext()
        {
            this->!RequestContext();

            delete _settings;
        }

        bool RegisterSchemeHandlerFactory(String^ schemeName, String^ domainName, ISchemeHandlerFactory^ factory)
        {
            auto wrapper = new SchemeHandlerFactoryWrapper(factory);
            return _requestContext->RegisterSchemeHandlerFactory(StringUtils::ToNative(schemeName), StringUtils::ToNative(domainName), wrapper);
        }

        bool ClearSchemeHandlerFactories()
        {
            return _requestContext->ClearSchemeHandlerFactories();
        }

        operator CefRefPtr<CefRequestContext>()
        {
            if(this == nullptr)
            {
                return NULL;
            }
            return _requestContext.get();
        }
    };
}
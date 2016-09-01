// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_scheme.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "Internals\CefSharpBrowserWrapper.h"
#include "Internals\CefFrameWrapper.h"
#include "Internals\CefRequestWrapper.h"
#include "ResourceHandlerWrapper.h"

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

        virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& schemeName, CefRefPtr<CefRequest> request) OVERRIDE
        {
            CefSharpBrowserWrapper browserWrapper(browser);
            CefFrameWrapper frameWrapper(frame);
            CefRequestWrapper requestWrapper(request);

            auto handler = _factory->Create(%browserWrapper, %frameWrapper, StringUtils::ToClr(schemeName), %requestWrapper);

            if (handler == nullptr)
            {
                return NULL;
            }

            if (handler->GetType() == ResourceHandler::typeid)
            {
                auto resourceHandler = static_cast<ResourceHandler^>(handler);
                if (resourceHandler->Type == ResourceHandlerType::File)
                {
                    return new CefStreamResourceHandler(StringUtils::ToNative(resourceHandler->MimeType), CefStreamReader::CreateForFile(StringUtils::ToNative(resourceHandler->FilePath)));
                }
            }

            return new ResourceHandlerWrapper(handler);
        }

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
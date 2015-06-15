// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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

        virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& schemeName, CefRefPtr<CefRequest> request) OVERRIDE
        {
            CefSharpBrowserWrapper browserWrapper(browser, nullptr);
            CefFrameWrapper frameWrapper(frame, nullptr);
            CefRequestWrapper requestWrapper(request);

            auto handler = _factory->Create(%browserWrapper, %frameWrapper, StringUtils::ToClr(schemeName), %requestWrapper);

            if (handler == nullptr)
            {
                return NULL;
            }

            return new ResourceHandlerWrapper(handler);
        }

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
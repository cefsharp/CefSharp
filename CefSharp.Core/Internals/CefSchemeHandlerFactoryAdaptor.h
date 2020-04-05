// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_scheme.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "Internals\CefBrowserWrapper.h"
#include "Internals\CefFrameWrapper.h"
#include "Request.h"
#include "ResourceHandlerWrapper.h"

using namespace System::IO;
using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        private class CefSchemeHandlerFactoryAdaptor : public CefSchemeHandlerFactory
        {
            gcroot<ISchemeHandlerFactory^> _factory;

        public:
            CefSchemeHandlerFactoryAdaptor(ISchemeHandlerFactory^ factory)
                : _factory(factory)
            {
            }

            ~CefSchemeHandlerFactoryAdaptor()
            {
                _factory = nullptr;
            }

            virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& schemeName, CefRefPtr<CefRequest> request) OVERRIDE
            {
                CefBrowserWrapper browserWrapper(browser);
                CefFrameWrapper frameWrapper(frame);
                Request requestWrapper(request);

                auto handler = _factory->Create(%browserWrapper, %frameWrapper, StringUtils::ToClr(schemeName), %requestWrapper);

                if (handler == nullptr)
                {
                    return NULL;
                }

                if (handler->GetType() == FileResourceHandler::typeid)
                {
                    auto resourceHandler = static_cast<FileResourceHandler^>(handler);

                    auto streamReader = CefStreamReader::CreateForFile(StringUtils::ToNative(resourceHandler->FilePath));

                    if (streamReader.get())
                    {
                        return new CefStreamResourceHandler(StringUtils::ToNative(resourceHandler->MimeType), streamReader);
                    }
                    else
                    {
                        auto msg = "Unable to load resource CefStreamReader::CreateForFile returned NULL for file:" + resourceHandler->FilePath;
                        LOG(ERROR) << StringUtils::ToNative(msg).ToString();

                        return NULL;
                    }
                }
                else if (handler->GetType() == ByteArrayResourceHandler::typeid)
                {
                    auto resourceHandler = static_cast<ByteArrayResourceHandler^>(handler);

                    //NOTE: Prefix with cli:: namespace as VS2015 gets confused with std::array
                    cli::array<Byte>^ buffer = resourceHandler->Data;
                    pin_ptr<Byte> src = &buffer[0];

                    auto streamReader = CefStreamReader::CreateForData(static_cast<void*>(src), buffer->Length);

                    return new CefStreamResourceHandler(StringUtils::ToNative(resourceHandler->MimeType), streamReader);
                }

                return new ResourceHandlerWrapper(handler);
            }

            IMPLEMENT_REFCOUNTING(CefSchemeHandlerFactoryAdaptor);
        };
    }
}

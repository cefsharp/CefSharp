// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#pragma once

#include "include/cef_scheme.h"
#include "IRequest.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    public interface class ISchemeHandler
    {
        /// <summary>
        /// if request is handled return true and set mimeType and stream accordingly.
        /// if no data the leave stream null
        /// </summary>
        bool ProcessRequest(IRequest^ request, String^% mimeType, Stream^% stream);
    };

    public interface class ISchemeHandlerFactory
    {
        ISchemeHandler^ Create();
    };

    class SchemeHandlerWrapper : public CefResourceHandler
    {
        gcroot<ISchemeHandler^> _handler;
        gcroot<Stream^> _stream;
        CefString _mime_type;

        int SizeFromStream();

    public:
        SchemeHandlerWrapper(ISchemeHandler^ handler) : _handler(handler)
        {
            if(!_handler)
            {
                throw gcnew ArgumentException("handler must not be null");
            }
        }

        virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback);
        virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl);
        virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback);
        virtual void Cancel();

        IMPLEMENT_LOCKING(SchemeHandlerWrapper);
        IMPLEMENT_REFCOUNTING(SchemeHandlerWrapper);
    };

    class SchemeHandlerFactoryWrapper : public CefSchemeHandlerFactory
    {
        gcroot<ISchemeHandlerFactory^> _factory;

    public:
        SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory)
            : _factory(factory) {}

        virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
            const CefString& scheme_name, CefRefPtr<CefRequest> request);

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
    ref class SchemeHandlerResponse;

    public class SchemeHandlerWrapper : public CefResourceHandler
    {
        CriticalSection _syncRoot;
        gcroot<ISchemeHandler^> _handler;
        gcroot<Stream^> _stream;
        CefRefPtr<CefCallback> _callback;
        CefString _mime_type;
        CefResponse::HeaderMap _headers;
        int _statusCode;
        CefString _redirectUrl;
        int _contentLength;
        bool _closeStream;
        int SizeFromStream();
    public:

        SchemeHandlerWrapper(ISchemeHandler^ handler) : _handler(handler)
        {
            if (static_cast<ISchemeHandler^>(_handler) == nullptr)
            {
                throw gcnew ArgumentException("handler must not be null");
            }
        }

        virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback);
        virtual void ProcessRequestCallback(SchemeHandlerResponse^ handlerResponse);
        virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl);
        virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback);
        virtual void Cancel();

        static CefResponse::HeaderMap ToHeaderMap(NameValueCollection^ headers);

        IMPLEMENT_REFCOUNTING(SchemeHandlerWrapper);
    };

    class SchemeHandlerFactoryWrapper : public CefSchemeHandlerFactory
    {
        gcroot<ISchemeHandlerFactory^> _factory;

    public:
        SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory)
            : _factory(factory) {}

        virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& scheme_name, CefRefPtr<CefRequest> request);

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
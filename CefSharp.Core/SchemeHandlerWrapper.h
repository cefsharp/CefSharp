// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_scheme.h"
#include "schemehandlerResponse.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    ref class SchemeHandlerResponseWrapper;

    public class SchemeHandlerWrapper : public CefResourceHandler
    {
    private:
        gcroot<ISchemeHandler^> _handler;
        gcroot<SchemeHandlerResponseWrapper^> _response; 
        CefRefPtr<CefCallback> _callback;
        void DeleteResponse();
        CefResponse::HeaderMap ToHeaderMap(IDictionary<String^, String^>^ headers);
    
    public:

        SchemeHandlerWrapper(ISchemeHandler^ handler) : 
            _handler(handler)
        {
            if (!_handler)
            {
                throw gcnew ArgumentException("handler must not be null");
            }
        };

        ~SchemeHandlerWrapper()
        {
            _handler = nullptr;
            _response = nullptr;
            _callback = nullptr;

            DeleteResponse();
        }

        void ProcessRequestCallback(Task^ previous);

        virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback) override;
        virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl) override;
        virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback) override;
        virtual void Cancel() override;

        IMPLEMENT_LOCKING(SchemeHandlerWrapper);
        IMPLEMENT_REFCOUNTING(SchemeHandlerWrapper);
    };

    class SchemeHandlerFactoryWrapper : public CefSchemeHandlerFactory
    {
        gcroot<ISchemeHandlerFactory^> _factory;

    public:
        SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory)
            : _factory(factory) 
        {
        };

        ~SchemeHandlerFactoryWrapper()
        {
            _factory = nullptr;
        };

        virtual CefRefPtr<CefResourceHandler> Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& scheme_name, CefRefPtr<CefRequest> request);

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
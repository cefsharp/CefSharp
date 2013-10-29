#include "Stdafx.h"
#pragma once

#include "include/cef_scheme.h"
#include "Request.h"
#include "ISchemeHandler.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    ref class SchemeHandlerResponse;

    public interface class ISchemeHandlerFactory
    {
        ISchemeHandler^ Create();
    };

    public class SchemeHandlerWrapper : public CefSchemeHandler
    {
        gcroot<ISchemeHandler^> _handler;
        gcroot<Stream^> _stream;
        CefString _mime_type;
        CefResponse::HeaderMap _headers;
        CefRefPtr<CefSchemeHandlerCallback> _callback;
        int _statusCode;
        CefString _redirectUrl;
        int _contentLength;
        bool _closeStream;

        int SizeFromStream();

    public:

        SchemeHandlerWrapper(ISchemeHandler^ handler) : _handler(handler)
        {
            if(!_handler)
            {
                throw gcnew ArgumentException("handler must not be null");
            }
        }

        virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefSchemeHandlerCallback> callback);
        virtual void ProcessRequestCallback(SchemeHandlerResponse^ handlerResponse);
        virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl);
        virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefSchemeHandlerCallback> callback);
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

        virtual CefRefPtr<CefSchemeHandler> Create(CefRefPtr<CefBrowser> browser, const CefString& scheme_name, CefRefPtr<CefRequest> request);

        IMPLEMENT_REFCOUNTING(SchemeHandlerFactoryWrapper);
    };
}
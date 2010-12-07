#include "stdafx.h"

#pragma once

#include "Request.h"

using namespace System;
using namespace System::IO;

namespace CefSharp {

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


class SchemeHandlerWrapper : public CefThreadSafeBase<CefSchemeHandler>
{
    gcroot<ISchemeHandler^> _handler;
    gcroot<Stream^> _stream;

    int SizeFromStream();

public:

    SchemeHandlerWrapper(ISchemeHandler^ handler) : _handler(handler) 
    {
        if(!_handler)
        {
            throw gcnew ArgumentException("handler must not be null");
        }
    }

    virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefString& mime_type, int* response_length);
    virtual void Cancel();
    virtual bool ReadResponse(void* data_out, int bytes_to_read, int* bytes_read);

};


class SchemeHandlerFactoryWrapper : public CefThreadSafeBase<CefSchemeHandlerFactory>
{
    gcroot<ISchemeHandlerFactory^> _factory;
   
public:
    SchemeHandlerFactoryWrapper(ISchemeHandlerFactory^ factory) 
        : _factory(factory) {}

    virtual CefRefPtr<CefSchemeHandler> Create();
};

};


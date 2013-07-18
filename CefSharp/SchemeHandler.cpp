// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "SchemeHandler.h"
#include "Internals/CefRequestWrapper.h"

namespace CefSharp
{
    void SchemeHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        response->SetMimeType(_mime_type);
        response->SetStatus(200);
        response_length = SizeFromStream();
    }

    bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        bool handled = false;
        Stream^ stream;
        String^ mimeType;

        AutoLock lock_scope(this);

        IRequest^ requestWrapper = gcnew CefRequestWrapper(request);
        
        if (_handler->ProcessRequest(requestWrapper, mimeType, stream))
        {
            _mime_type = StringUtils::ToNative(mimeType);
            _stream = stream;
            callback->Continue();

            handled = true;
        }

        // TODO: We should have a way to make the SchemeHandler call callback->Cancel(). Perhaps we should always set handled to
        // true, but call Cancel() if the result of _handler->ProcessRequest() is false...

        return handled;
    }

    bool SchemeHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback)
    {
        bool has_data = false;

        AutoLock lock_scope(this);

        if(!_stream)
        {
            bytes_read = 0;
        }
        else
        {
            array<Byte>^ buffer = gcnew array<Byte>(bytes_to_read);
            int ret = _stream->Read(buffer, 0, bytes_to_read);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(data_out, static_cast<void*>(src), ret);
            bytes_read = ret;
            has_data = true;
        }

        return has_data;
    }

    void SchemeHandlerWrapper::Cancel()
    {
        _stream = nullptr;
    }

    int SchemeHandlerWrapper::SizeFromStream()
    {
        if(!_stream)
        {
            return 0;
        }

        if(_stream->CanSeek)
        {
            _stream->Seek(0, SeekOrigin::End);
            int length = static_cast<int>(_stream->Position);
            _stream->Seek(0, SeekOrigin::Begin);
            return length;
        }
        return -1;
    }

    CefRefPtr<CefResourceHandler> SchemeHandlerFactoryWrapper::Create(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
        const CefString& scheme_name, CefRefPtr<CefRequest> request)
    {
        ISchemeHandler^ handler = _factory->Create();
        CefRefPtr<SchemeHandlerWrapper> wrapper = new SchemeHandlerWrapper(handler);
        return static_cast<CefRefPtr<CefResourceHandler>>(wrapper);
    }
}

#include "stdafx.h"

#include "SchemeHandler.h"

namespace CefSharp
{
    void SchemeHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        response->SetMimeType(_mime_type);
        response->SetStatus(200);
        response_length = SizeFromStream();
    }

    bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefSchemeHandlerCallback> callback)
    {
        bool handled = false;
        Stream^ stream;
        String^ mimeType;

        AutoLock lock_scope(this);

        IRequest^ requestWrapper = gcnew CefRequestWrapper(request);
        if (_handler->ProcessRequest(requestWrapper, mimeType, stream))
        {
            _mime_type = toNative(mimeType);
            _stream = stream;
            callback->HeadersAvailable();

            handled = true;
        }

        return handled;
    }

    bool SchemeHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefSchemeHandlerCallback> callback)
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

    CefRefPtr<CefSchemeHandler> SchemeHandlerFactoryWrapper::Create(CefRefPtr<CefBrowser> browser, const CefString& scheme_name, CefRefPtr<CefRequest> request)
    {
        ISchemeHandler^ handler = _factory->Create();
        CefRefPtr<SchemeHandlerWrapper> wrapper = new SchemeHandlerWrapper(handler);
        return static_cast<CefRefPtr<CefSchemeHandler>>(wrapper);
    }
}
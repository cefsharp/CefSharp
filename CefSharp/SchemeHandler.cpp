#include "stdafx.h"

#include "SchemeHandler.h"

namespace CefSharp
{

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

    bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> cefRequest, CefString& redirectUrl, CefRefPtr<CefResponse> response, int* responseLength)
    //bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> cefRequest, CefString &cefMimeType, int *responseLength)
    {
        bool handled = false;
        
        Lock();

        IRequest^ requestWrapper = gcnew CefRequestWrapper(cefRequest);

        Stream^ stream;
        String^ mimeType;

        handled = _handler->ProcessRequest(requestWrapper, mimeType, stream);
        _stream = stream;

        /*
        if(mimeType != nullptr)
        {
            cefMimeType = toNative(mimeType);
        }
        */
        
        *responseLength = SizeFromStream();
        
        Unlock();
        return handled;
    }

    void SchemeHandlerWrapper::Cancel()
    {
        _stream = nullptr;
    }

    bool SchemeHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int* bytes_read)
    {
        bool has_data = false;

        Lock();

        if(!_stream)
        {
            *bytes_read = 0;            
        }
        else
        {
            array<Byte>^ buffer = gcnew array<Byte>(bytes_to_read);
            int ret = _stream->Read(buffer, 0, bytes_to_read);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(data_out, static_cast<void*>(src), ret);
            *bytes_read = ret;
            has_data = true;
        }

        Unlock();

        return has_data;
    }

    CefRefPtr<CefSchemeHandler> SchemeHandlerFactoryWrapper::Create()
    {
        ISchemeHandler^ handler = _factory->Create();
        CefRefPtr<SchemeHandlerWrapper> wrapper = new SchemeHandlerWrapper(handler);
        return static_cast<CefRefPtr<CefSchemeHandler>>(wrapper);
    }

}
// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/CefRequestWrapper.h"
#include "SchemeHandlerWrapper.h"
#include "SchemeHandlerResponse.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
    CefResponse::HeaderMap SchemeHandlerWrapper::ToHeaderMap(IDictionary<String^, String^>^ headers)
    {
        CefResponse::HeaderMap result;

        if (headers == nullptr)
        {
            return result;
        }

        for each (KeyValuePair<String^, String^> header in headers)
        {
            result.insert(std::pair<CefString, CefString>(StringUtils::ToNative(header.Key), StringUtils::ToNative(header.Value)));
        }

        return result;
    }
    
    void SchemeHandlerWrapper::DeleteResponse()
    {
        SchemeHandlerResponseWrapper^ response = _response;
        _response = nullptr;
        if (response != nullptr)
        {
            delete response;
        }

        _callback = nullptr;
    }

    bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        bool handled = false;
        
        _callback = callback;
        _response = gcnew SchemeHandlerResponseWrapper(this);
                
        auto onRequestCompleted = gcnew Action<Task^>(_response, &SchemeHandlerResponseWrapper::OnRequestCompleted);
        auto requestWrapper = gcnew CefRequestWrapper(request);

        auto task = _handler->ProcessRequestAsync(requestWrapper, _response);
        if (task != nullptr)
        {
            task->ContinueWith(onRequestCompleted);
            handled = true;
        }
        else
        {
            DeleteResponse();
        }

        return handled;
    }

    void SchemeHandlerWrapper::ProcessRequestCallback(Task^ previous)
    {
        _callback->Continue();        
    }

    void SchemeHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        response->SetMimeType(StringUtils::ToNative(_response->MimeType));
        response->SetStatus((int)_response->StatusCode );
        response->SetHeaderMap(ToHeaderMap(_response->ResponseHeaders));
        if (_response->ContentLength >= 0)
        {
            response_length = _response->ContentLength;
        }
        else
        {
            response_length = _response->SizeFromStream();
        }
        redirectUrl = StringUtils::ToNative(_response->RedirectUrl);
    }

    bool SchemeHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback)
    {
        bool has_data = false;
        
        if (!_response->ResponseStream)
        {
            bytes_read = 0;
        }
        else
        {
            array<Byte>^ buffer = gcnew array<Byte>(bytes_to_read);
            int ret = _response->ResponseStream->Read(buffer, 0, bytes_to_read);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(data_out, static_cast<void*>(src), ret);
            bytes_read = ret;
            // must return false when the response is complete
            has_data = ret > 0;
            if (!has_data && _response->CloseStream)
            {
                _response->ResponseStream->Close();
            }
        }

        return has_data;
    }

    void SchemeHandlerWrapper::Cancel()
    {        
        if (static_cast<SchemeHandlerResponseWrapper^>(_response) != nullptr 
            && _response->ResponseStream != nullptr 
            && _response->CloseStream)
        {
            _response->ResponseStream->Close();
        }

        DeleteResponse();
    }

    

    CefRefPtr<CefResourceHandler> SchemeHandlerFactoryWrapper::Create(
        CefRefPtr<CefBrowser> browser,
        CefRefPtr<CefFrame> frame,
        const CefString& scheme_name,
        CefRefPtr<CefRequest> request)
    {
        ISchemeHandler^ handler = _factory->Create();
        CefRefPtr<SchemeHandlerWrapper> wrapper = new SchemeHandlerWrapper(handler);
        return static_cast<CefRefPtr<CefResourceHandler>>(wrapper);
    }
}
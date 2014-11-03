// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
    CefResponse::HeaderMap SchemeHandlerWrapper::ToHeaderMap(NameValueCollection^ headers)
    {
        CefResponse::HeaderMap result;

        if (headers == nullptr)
        {
            return result;
        }

        for each (String^ key in headers)
        {
            String^ value = headers[key];
            result.insert(std::pair<CefString, CefString>(StringUtils::ToNative(key), StringUtils::ToNative(value)));
        }

        return result;
    }

    bool SchemeHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        _callback = callback;

        bool handled = false;

        AutoLock lock_scope(_syncRoot);

        auto schemeResponse = gcnew SchemeHandlerResponse(this);
        auto onRequestCompleted = gcnew OnRequestCompletedHandler(schemeResponse, &SchemeHandlerResponse::OnRequestCompleted);

        auto requestWrapper = gcnew CefRequestWrapper(request);
        if (_handler->ProcessRequestAsync(requestWrapper, schemeResponse, onRequestCompleted))
        {
            handled = true;
        }

        return handled;
    }

    void SchemeHandlerWrapper::ProcessRequestCallback(SchemeHandlerResponse^ response)
    {
        _mime_type = StringUtils::ToNative(response->MimeType);
        _stream = response->ResponseStream;
        _statusCode = response->StatusCode;
        _redirectUrl = StringUtils::ToNative(response->RedirectUrl);
        _contentLength = response->ContentLength;
        _closeStream = response->CloseStream;

        _headers = ToHeaderMap(response->ResponseHeaders);

        // If CEF has cancelled the initial request, throw away a response that comes afterwards.
        if (_callback != nullptr)
        {
            _callback->Continue();
        }

        // Must be done AFTER CEF has been allowed to consume the headers etc. After this call is made, the SchemeHandlerWrapper
        // instance has likely been deallocated.
        response->ReleaseSchemeHandlerWrapper();
    }

    void SchemeHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        response->SetMimeType(_mime_type);
        response->SetStatus(_statusCode > 0 ? _statusCode : 200);
        response->SetHeaderMap(_headers);
        if (_contentLength >= 0)
        {
            response_length = _contentLength;
        }
        else
        {
            response_length = SizeFromStream();
        }
        redirectUrl = _redirectUrl;
    }

    bool SchemeHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback)
    {
        bool has_data = false;

        AutoLock lock_scope(_syncRoot);

        if (static_cast<Stream^>(_stream) == nullptr)
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
            // must return false when the response is complete
            has_data = ret > 0;
            if (!has_data && _closeStream)
            {
                _stream->Close();
            }
        }

        return has_data;
    }

    void SchemeHandlerWrapper::Cancel()
    {
        if (static_cast<Stream^>(_stream) != nullptr && _closeStream)
        {
            _stream->Close();
        }
        _stream = nullptr;
        _callback = NULL;
    }

    int SchemeHandlerWrapper::SizeFromStream()
    {
        if (static_cast<Stream^>(_stream) == nullptr)
        {
            return 0;
        }

        if (_stream->CanSeek)
        {
            _stream->Seek(0, SeekOrigin::End);
            int length = static_cast<int>(_stream->Position);
            _stream->Seek(0, SeekOrigin::Begin);
            return length;
        }
        return -1;
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
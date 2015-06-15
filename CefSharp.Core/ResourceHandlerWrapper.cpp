// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/CefRequestWrapper.h"
#include "ResourceHandlerWrapper.h"
#include "ResourceHandlerResponse.h"
#include "Internals/TypeConversion.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
    bool ResourceHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        _callback = callback;

        AutoLock lock_scope(_syncRoot);

        auto schemeResponse = gcnew ResourceHandlerResponse(this);

        // If _request came from ISchemeHandlerFactory,
        // the request parameter can be ignored since it is the same request.
        // If ignoring the request parameter isn't good enough then we 
        // should retain references to _requestWrapper and a CefRequestWrapper^
        // of the request parameter until CefRequestWrapper is disposed.
        if (static_cast<IRequest^>(_request) == nullptr)
        {
            _request = gcnew CefRequestWrapper(request);
        }

        return _handler->ProcessRequestAsync(_request, schemeResponse);
    }

    void ResourceHandlerWrapper::ProcessRequestCallback(IResourceHandlerResponse^ response, bool cancel)
    {
        // If CEF has canceled the initial request, throw away a response that comes afterwards.
        if (_callback != nullptr)
        {
            if(cancel)
            {
                _callback->Cancel();
            }
            else
            {
                _mime_type = StringUtils::ToNative(response->MimeType);
                _stream = response->ResponseStream;
                _statusCode = response->StatusCode;
                _statusText = StringUtils::ToNative(response->StatusText);
                _redirectUrl = StringUtils::ToNative(response->RedirectUrl);
                _contentLength = response->ContentLength;
                _closeStream = response->CloseStream;

                _headers = TypeConversion::ToNative(response->ResponseHeaders);

                _callback->Continue();
            }
        }

        // Must be done AFTER CEF has been allowed to consume the headers etc. After this call is made, the SchemeHandlerWrapper
        // instance has likely been deallocated.
        delete response;
    }

    void ResourceHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        response->SetMimeType(_mime_type);
        response->SetStatus(_statusCode);
        response->SetStatusText(_statusText);
        response->SetHeaderMap(_headers);
        // ContentLength defaults to -1 so SizeFromStream is called
        response_length = _contentLength >= 0 ? _contentLength : SizeFromStream();
        
        redirectUrl = _redirectUrl;
    }

    bool ResourceHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback)
    {
        bool hasData = false;

        AutoLock lock_scope(_syncRoot);

        if (static_cast<Stream^>(_stream) == nullptr)
        {
            bytes_read = 0;
        }
        else
        {
            array<Byte>^ buffer = gcnew array<Byte>(bytes_to_read);
            bytes_read = _stream->Read(buffer, 0, bytes_to_read);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(data_out, static_cast<void*>(src), bytes_read);
            // must return false when the response is complete
            hasData = bytes_read > 0;
            if (!hasData && _closeStream)
            {
                _stream->Close();
            }
        }

        return hasData;
    }

    void ResourceHandlerWrapper::Cancel()
    {
        if (static_cast<Stream^>(_stream) != nullptr && _closeStream)
        {
            _stream->Close();
        }
        _stream = nullptr;
        _callback = NULL;
    }

    int64 ResourceHandlerWrapper::SizeFromStream()
    {
        if (static_cast<Stream^>(_stream) == nullptr)
        {
            return 0;
        }

        if (_stream->CanSeek)
        {
            _stream->Seek(0, System::IO::SeekOrigin::End);
            int64 length = static_cast<int>(_stream->Position);
            _stream->Seek(0, System::IO::SeekOrigin::Begin);
            return length;
        }
        return -1;
    }
}
// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefResponseWrapper.h"
#include "Internals/CefCallbackWrapper.h"
#include "ResourceHandlerWrapper.h"
#include "Internals/TypeConversion.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
    bool ResourceHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);

        // If we already have a non-null _request
        // dispose it via delete before using the parameter for the rest
        // of this object's lifetime. This ought to be sensible to do
        // because the contained data ought to be nearly identical.
        delete _request;

        _request = gcnew CefRequestWrapper(request);

        return _handler->ProcessRequestAsync(_request, callbackWrapper);
    }

    void ResourceHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        String^ newRedirectUrl;

        CefResponseWrapper responseWrapper(response);

        _stream = _handler->GetResponse(%responseWrapper, response_length, newRedirectUrl);

        redirectUrl = StringUtils::ToNative(newRedirectUrl);
    }

    bool ResourceHandlerWrapper::ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback)
    {
        bool hasData = false;

        if (static_cast<Stream^>(_stream) == nullptr)
        {
            bytes_read = 0;
        }
        else
        {
            auto buffer = gcnew cli::array<Byte>(bytes_to_read);
            bytes_read = _stream->Read(buffer, 0, bytes_to_read);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(data_out, static_cast<void*>(src), bytes_read);
            // must return false when the response is complete
            hasData = bytes_read > 0;
            //TODO: Fix this
            /*if (!hasData && _closeStream)
            {
                _stream->Close();
            }*/
        }

        return hasData;
    }

    bool ResourceHandlerWrapper::CanGetCookie(const CefCookie& cookie)
    {
        //Default value is true
        return true;
    }

    bool ResourceHandlerWrapper::CanSetCookie(const CefCookie& cookie)
    {
        //Default value is true
        return true;
    }

    void ResourceHandlerWrapper::Cancel()
    {
        //TODO: Fix this
        /*if (static_cast<Stream^>(_stream) != nullptr && _closeStream)
        {
            _stream->Close();
        }*/
        _stream = nullptr;

        // Do not dispose here; since CEF 2537 the ResourceHandlerWrapper pointer is
        // referenced after Cancel and disposal would cause an access violation.
        //delete this;
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
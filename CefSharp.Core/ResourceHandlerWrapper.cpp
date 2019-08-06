// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefResponseWrapper.h"
#include "Internals/CefCallbackWrapper.h"
#include "Internals/CefResourceReadCallbackWrapper.h"
#include "Internals/CefResourceSkipCallbackWrapper.h"
#include "Internals/TypeConversion.h"
#include "ResourceHandlerWrapper.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
    bool ResourceHandlerWrapper::Open(CefRefPtr<CefRequest> request, bool& handleRequest, CefRefPtr<CefCallback> callback)
    {
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);
        _request = gcnew CefRequestWrapper(request);

        return _handler->Open(_request, handleRequest, callbackWrapper);
    }

    void ResourceHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        String^ newRedirectUrl;

        CefResponseWrapper responseWrapper(response);

        _handler->GetResponseHeaders(%responseWrapper, response_length, newRedirectUrl);

        redirectUrl = StringUtils::ToNative(newRedirectUrl);
    }

    bool ResourceHandlerWrapper::Skip(int64 bytesToSkip, int64& bytesSkipped, CefRefPtr<CefResourceSkipCallback> callback)
    {
        auto callbackWrapper = gcnew CefResourceSkipCallbackWrapper(callback);

        return _handler->Skip(bytesToSkip, bytesSkipped, callbackWrapper);
    }

    bool ResourceHandlerWrapper::Read(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefResourceReadCallback> callback)
    {
        auto writeStream = gcnew UnmanagedMemoryStream((Byte*)dataOut, (Int64)bytesToRead, (Int64)bytesToRead, FileAccess::Write);
        auto callbackWrapper = gcnew CefResourceReadCallbackWrapper(callback);

        return _handler->Read(writeStream, bytesRead, callbackWrapper);
    }

    void ResourceHandlerWrapper::Cancel()
    {
        _handler->Cancel();

        delete _request;
        _request = nullptr;
    }

    //Deprecated
    bool ResourceHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);
        _request = gcnew CefRequestWrapper(request);

        return _handler->ProcessRequest(_request, callbackWrapper);
    }

    bool ResourceHandlerWrapper::ReadResponse(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefCallback> callback)
    {
        auto writeStream = gcnew UnmanagedMemoryStream((Byte*)dataOut, (Int64)bytesToRead, (Int64)bytesToRead, FileAccess::Write);
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);

        return _handler->ReadResponse(writeStream, bytesRead, callbackWrapper);
    }
}

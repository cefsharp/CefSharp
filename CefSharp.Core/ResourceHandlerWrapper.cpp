// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefResponseWrapper.h"
#include "Internals/CefCallbackWrapper.h"
#include "Internals/TypeConversion.h"
#include "ResourceHandlerWrapper.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

namespace CefSharp
{
    bool ResourceHandlerWrapper::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
    {
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);
        _request = gcnew CefRequestWrapper(request);

        return _handler->ProcessRequest(_request, callbackWrapper);
    }

    void ResourceHandlerWrapper::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
    {
        String^ newRedirectUrl;

        CefResponseWrapper responseWrapper(response);

        _handler->GetResponseHeaders(%responseWrapper, response_length, newRedirectUrl);

        redirectUrl = StringUtils::ToNative(newRedirectUrl);
    }

    bool ResourceHandlerWrapper::ReadResponse(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefCallback> callback)
    {
        UnmanagedMemoryStream writeStream((Byte*)dataOut, (Int64)bytesToRead, (Int64)bytesToRead, FileAccess::Write);
        auto callbackWrapper = gcnew CefCallbackWrapper(callback);

        return _handler->ReadResponse(%writeStream, bytesRead, callbackWrapper);
    }

    bool ResourceHandlerWrapper::CanGetCookie(const CefCookie& cefCookie)
    {
        auto cookie = GetCookie(cefCookie);

        //Default value is true
        return _handler->CanGetCookie(cookie);
    }

    bool ResourceHandlerWrapper::CanSetCookie(const CefCookie& cefCookie)
    {
        auto cookie = GetCookie(cefCookie);

        //Default value is true
        return _handler->CanSetCookie(cookie);
    }

    void ResourceHandlerWrapper::Cancel()
    {
        _handler->Cancel();
    }

    Cookie^ ResourceHandlerWrapper::GetCookie(const CefCookie& cefCookie)
    {
        return TypeConversion::FromNative(cefCookie);
    }
}

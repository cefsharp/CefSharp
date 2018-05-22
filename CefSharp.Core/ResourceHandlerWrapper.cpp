// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        auto cookie = gcnew Cookie();
        String^ cookieName = StringUtils::ToClr(cefCookie.name);

        if (!String::IsNullOrEmpty(cookieName))
        {
            cookie->Name = StringUtils::ToClr(cefCookie.name);
            cookie->Value = StringUtils::ToClr(cefCookie.value);
            cookie->Domain = StringUtils::ToClr(cefCookie.domain);
            cookie->Path = StringUtils::ToClr(cefCookie.path);
            cookie->Secure = cefCookie.secure == 1;
            cookie->HttpOnly = cefCookie.httponly == 1;

            if (cefCookie.has_expires)
            {
                auto expires = cefCookie.expires;
                cookie->Expires = DateTimeUtils::FromCefTime(expires.year,
                    expires.month,
                    expires.day_of_month,
                    expires.hour,
                    expires.minute,
                    expires.second,
                    expires.millisecond);
            }

            auto creation = cefCookie.creation;
            cookie->Creation = DateTimeUtils::FromCefTime(creation.year,
                creation.month,
                creation.day_of_month,
                creation.hour,
                creation.minute,
                creation.second,
                creation.millisecond);

            auto lastAccess = cefCookie.last_access;
            cookie->LastAccess = DateTimeUtils::FromCefTime(lastAccess.year,
                lastAccess.month,
                lastAccess.day_of_month,
                lastAccess.hour,
                lastAccess.minute,
                lastAccess.second,
                lastAccess.millisecond);
        }

        return cookie;
    }
}
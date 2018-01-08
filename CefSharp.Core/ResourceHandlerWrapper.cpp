﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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

			try
			{
				if (cefCookie.has_expires)
				{
					cookie->Expires = DateTime(
						cefCookie.expires.year,
						cefCookie.expires.month,
						cefCookie.expires.day_of_month,
						cefCookie.expires.hour,
						cefCookie.expires.minute,
						cefCookie.expires.second,
						cefCookie.expires.millisecond
						);
				}
			}
			catch (Exception^ ex)
			{
				cookie->Expires = DateTime::MinValue;
			}

            //TODO: There is a method in TypeUtils that's in BrowserSubProcess that convers CefTime, need to make it accessible.
			try
			{
				cookie->Creation = DateTime(
					cefCookie.creation.year,
					cefCookie.creation.month,
					cefCookie.creation.day_of_month,
					cefCookie.creation.hour,
					cefCookie.creation.minute,
					cefCookie.creation.second,
					cefCookie.creation.millisecond
					);
			}
			catch (Exception^ ex)
			{
				cookie->Creation = DateTime::MinValue;
			}

			try
			{
				cookie->LastAccess = DateTime(
					cefCookie.last_access.year,
					cefCookie.last_access.month,
					cefCookie.last_access.day_of_month,
					cefCookie.last_access.hour,
					cefCookie.last_access.minute,
					cefCookie.last_access.second,
					cefCookie.last_access.millisecond
					);
			}
			catch (Exception^ ex)
			{
				cookie->LastAccess = DateTime::MinValue;
			}
        }

        return cookie;
    }
}

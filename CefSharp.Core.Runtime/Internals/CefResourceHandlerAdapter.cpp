// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Request.h"
#include "CefResourceHandlerAdapter.h"
#include "Internals/CefResponseWrapper.h"
#include "Internals/CefCallbackWrapper.h"
#include "Internals/CefResourceReadCallbackWrapper.h"
#include "Internals/CefResourceSkipCallbackWrapper.h"
#include "Internals/TypeConversion.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;

using namespace CefSharp::Core;

namespace CefSharp
{
    namespace Internals
    {
        bool CefResourceHandlerAdapter::Open(CefRefPtr<CefRequest> request, bool& handleRequest, CefRefPtr<CefCallback> callback)
        {
            auto callbackWrapper = gcnew CefCallbackWrapper(callback);
            _request = gcnew Request(request);

            return _handler->Open(_request, handleRequest, callbackWrapper);
        }

        void CefResourceHandlerAdapter::GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl)
        {
            String^ newRedirectUrl;

            CefResponseWrapper responseWrapper(response);

            _handler->GetResponseHeaders(% responseWrapper, response_length, newRedirectUrl);

            redirectUrl = StringUtils::ToNative(newRedirectUrl);
        }

        bool CefResourceHandlerAdapter::Skip(int64 bytesToSkip, int64& bytesSkipped, CefRefPtr<CefResourceSkipCallback> callback)
        {
            auto callbackWrapper = gcnew CefResourceSkipCallbackWrapper(callback);

            return _handler->Skip(bytesToSkip, bytesSkipped, callbackWrapper);
        }

        bool CefResourceHandlerAdapter::Read(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefResourceReadCallback> callback)
        {
            auto writeStream = gcnew UnmanagedMemoryStream((Byte*)dataOut, (Int64)bytesToRead, (Int64)bytesToRead, FileAccess::Write);
            auto callbackWrapper = gcnew CefResourceReadCallbackWrapper(callback);

            return _handler->Read(writeStream, bytesRead, callbackWrapper);
        }

        void CefResourceHandlerAdapter::Cancel()
        {
            _handler->Cancel();

            delete _request;
            _request = nullptr;
        }

        //Deprecated
        bool CefResourceHandlerAdapter::ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback)
        {
            auto callbackWrapper = gcnew CefCallbackWrapper(callback);
            _request = gcnew Request(request);

            return _handler->ProcessRequest(_request, callbackWrapper);
        }

        bool CefResourceHandlerAdapter::ReadResponse(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefCallback> callback)
        {
            auto writeStream = gcnew UnmanagedMemoryStream((Byte*)dataOut, (Int64)bytesToRead, (Int64)bytesToRead, FileAccess::Write);
            auto callbackWrapper = gcnew CefCallbackWrapper(callback);

            return _handler->ReadResponse(writeStream, bytesRead, callbackWrapper);
        }
    }
}

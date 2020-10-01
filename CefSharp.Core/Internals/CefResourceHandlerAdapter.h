// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_scheme.h"

using namespace System::IO;
using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        public class CefResourceHandlerAdapter : public CefResourceHandler
        {
        private:
            gcroot<IResourceHandler^> _handler;
            gcroot<IRequest^> _request;
        public:
            CefResourceHandlerAdapter(IResourceHandler^ handler)
                : _handler(handler)
            {
            }

            ~CefResourceHandlerAdapter()
            {
                delete _handler;
                _handler = nullptr;

                delete _request;
                _request = nullptr;
            }

            virtual bool Open(CefRefPtr<CefRequest> request, bool& handle_request, CefRefPtr<CefCallback> callback) OVERRIDE;
            virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl) OVERRIDE;
            virtual bool Skip(int64 bytesToSkip, int64& bytesSkipped, CefRefPtr<CefResourceSkipCallback> callback) OVERRIDE;
            virtual bool Read(void* dataOut, int bytesToRead, int& bytesRead, CefRefPtr<CefResourceReadCallback> callback) OVERRIDE;
            virtual void Cancel() OVERRIDE;

            //Depricated
            virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback) OVERRIDE;
            virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback) OVERRIDE;

            IMPLEMENT_REFCOUNTING(CefResourceHandlerAdapter);
        };
    }
}

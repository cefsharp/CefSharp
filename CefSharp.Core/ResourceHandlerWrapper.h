// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_scheme.h"

using namespace System::IO;
using namespace System::Collections::Specialized;

namespace CefSharp
{
    public class ResourceHandlerWrapper : public CefResourceHandler
    {
    private:
        gcroot<IResourceHandler^> _handler;

        Cookie^ GetCookie(const CefCookie& cookie);

    public:

        /// <summary>
        /// Constructor that accepts IBrowser, IFrame, IRequest in order to be the CefSharp
        /// lifetime management container  (i.e. calling .Dispose at the correct time) on 
        /// managed objects that contain MCefRefPtrs.
        /// </summary>
        ResourceHandlerWrapper(IResourceHandler^ handler)
            : _handler(handler)
        {
        }

        ~ResourceHandlerWrapper()
        {
            delete _handler;
            _handler = nullptr;
        }

        virtual bool ProcessRequest(CefRefPtr<CefRequest> request, CefRefPtr<CefCallback> callback) OVERRIDE;
        virtual void GetResponseHeaders(CefRefPtr<CefResponse> response, int64& response_length, CefString& redirectUrl) OVERRIDE;
        virtual bool ReadResponse(void* data_out, int bytes_to_read, int& bytes_read, CefRefPtr<CefCallback> callback) OVERRIDE;
        virtual bool CanGetCookie(const CefCookie& cookie) OVERRIDE;
        virtual bool CanSetCookie(const CefCookie& cookie) OVERRIDE;
        virtual void Cancel() OVERRIDE;

        IMPLEMENT_REFCOUNTING(ResourceHandlerWrapper);
    };
}
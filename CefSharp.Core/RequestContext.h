// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals\MCefRefPtr.h"
#include "RequestContextSettings.h"
#include "include\cef_request_context.h"

using namespace CefSharp;

namespace CefSharp
{
    public ref class RequestContext
    {
    private:
        MCefRefPtr<CefRequestContext> _requestContext;
    public:
        RequestContext()
        {
            CefRequestContextSettings settings;
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        RequestContext(RequestContextSettings^ settings)
        {
            _requestContext = CefRequestContext::CreateContext(settings, NULL);
        }

        !RequestContext()
        {
            _requestContext = NULL;
        }

        ~RequestContext()
        {
            this->!RequestContext();
        }

        operator CefRefPtr<CefRequestContext>()
        {
            return _requestContext.get();
        }
    };
}
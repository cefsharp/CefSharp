// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_request_context.h"

namespace CefSharp
{
    private class CefResolveCallbackAdapter : public CefResolveCallback
    {
    private:
        gcroot<IResolveCallback^> _handler;

    public:
        CefResolveCallbackAdapter(IResolveCallback^ handler)
        {
            _handler = handler;
        }

        ~CefResolveCallbackAdapter()
        {
            _handler = nullptr;
        }

        void OnResolveCompleted(cef_errorcode_t result,	const std::vector<CefString>& resolvedIps) OVERRIDE
        {
            _handler->OnResolveCompleted((CefErrorCode)result, StringUtils::ToClr(resolvedIps));
        }

        IMPLEMENT_REFCOUNTING(CefResolveCallbackAdapter);
    };
}
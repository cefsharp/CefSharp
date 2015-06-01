// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_auth_callback.h"

namespace CefSharp
{
    public ref class CefAuthCallbackWrapper : IAuthCallback
    {
    private:
        MCefRefPtr<CefAuthCallback> _callback;

    public:
        CefAuthCallbackWrapper(CefRefPtr<CefAuthCallback> &callback) : _callback(callback)
        {
            
        }

        ~CefAuthCallbackWrapper()
        {
            _callback = NULL;
        }

        virtual void Cancel()
        {
            _callback->Cancel();
        }

        virtual void Continue(String^ username, String^ password)
        {
            _callback->Continue(StringUtils::ToNative(username), StringUtils::ToNative(password));
        }
    };
}
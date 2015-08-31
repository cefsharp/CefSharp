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
        IFrame^ _frame;
        bool _disposed;

    public:
        CefAuthCallbackWrapper(CefRefPtr<CefAuthCallback> &callback, IFrame^ frame)
            : _callback(callback), _frame(frame), _disposed(false)
        {
            
        }

        !CefAuthCallbackWrapper()
        {
            _callback = NULL;
        }

        ~CefAuthCallbackWrapper()
        {
            this->!CefAuthCallbackWrapper();
            delete _frame;
            _frame = nullptr;

            _disposed = true;
        }

        virtual void Cancel()
        {
            _callback->Cancel();

            delete this;
        }

        virtual void Continue(String^ username, String^ password)
        {
            _callback->Continue(StringUtils::ToNative(username), StringUtils::ToNative(password));

            delete this;
        }

        virtual property bool IsDisposed
        {
            bool get()
            {
                return _disposed;
            }
        }
    };
}
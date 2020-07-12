// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_auth_callback.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefAuthCallbackWrapper : public IAuthCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefAuthCallback> _callback;

        public:
            CefAuthCallbackWrapper(CefRefPtr<CefAuthCallback> &callback)
                : _callback(callback)
            {

            }

            !CefAuthCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefAuthCallbackWrapper()
            {
                this->!CefAuthCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Continue(String^ username, String^ password)
            {
                ThrowIfDisposed();

                _callback->Continue(StringUtils::ToNative(username), StringUtils::ToNative(password));

                delete this;
            }
        };
    }
}

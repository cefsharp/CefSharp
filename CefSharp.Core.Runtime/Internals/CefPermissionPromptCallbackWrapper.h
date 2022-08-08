// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_permission_handler.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefPermissionPromptCallbackWrapper : public IPermissionPromptCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefPermissionPromptCallback> _callback;
        public:
            CefPermissionPromptCallbackWrapper(CefRefPtr<CefPermissionPromptCallback> &callback):
            _callback(callback)
            {
                
            }

            !CefPermissionPromptCallbackWrapper()
            {
                _callback = nullptr;
            }

            ~CefPermissionPromptCallbackWrapper()
            {
                this->!CefPermissionPromptCallbackWrapper();

                _disposed = true;
            }

            virtual void Continue(PermissionRequestResult result)
            {
                ThrowIfDisposed();

                _callback->Continue(static_cast<cef_permission_request_result_t>(result));

                delete this;
            }
        };
    }
}

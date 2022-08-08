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
        private ref class CefMediaAccessCallbackWrapper : public IMediaAccessCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefMediaAccessCallback> _callback;
        public:
            CefMediaAccessCallbackWrapper(CefRefPtr<CefMediaAccessCallback>& callback) :
                _callback(callback)
            {

            }

            !CefMediaAccessCallbackWrapper()
            {
                _callback = nullptr;
            }

            ~CefMediaAccessCallbackWrapper()
            {
                this->!CefMediaAccessCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Continue(MediaAccessPermissionType allowedPermissions)
            {
                ThrowIfDisposed();

                _callback->Continue(static_cast<cef_media_access_permission_types_t>(allowedPermissions));

                delete this;
            }            
        };
    }
}

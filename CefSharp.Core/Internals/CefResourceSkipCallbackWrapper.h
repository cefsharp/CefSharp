// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_resource_handler.h"
#include "CefWrapper.h"

using namespace CefSharp::Callback;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefResourceSkipCallbackWrapper : public IResourceSkipCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefResourceSkipCallback> _callback;

        public:
            CefResourceSkipCallbackWrapper(CefRefPtr<CefResourceSkipCallback> &callback) :
                _callback(callback)
            {

            }

            !CefResourceSkipCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefResourceSkipCallbackWrapper()
            {
                this->!CefResourceSkipCallbackWrapper();

                _disposed = true;
            }

            virtual void Continue(Int64 bytesSkipped)
            {
                ThrowIfDisposed();

                _callback->Continue(bytesSkipped);

                delete this;
            }
        };
    }
}

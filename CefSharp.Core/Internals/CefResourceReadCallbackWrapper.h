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
        private ref class CefResourceReadCallbackWrapper : public IResourceReadCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefResourceReadCallback> _callback;

        public:
            CefResourceReadCallbackWrapper(CefRefPtr<CefResourceReadCallback> &callback) :
                _callback(callback)
            {

            }

            !CefResourceReadCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefResourceReadCallbackWrapper()
            {
                this->!CefResourceReadCallbackWrapper();

                _disposed = true;
            }

            virtual void Continue(int bytesRead)
            {
                ThrowIfDisposed();

                _callback->Continue(bytesRead);

                delete this;
            }
        };
    }
}

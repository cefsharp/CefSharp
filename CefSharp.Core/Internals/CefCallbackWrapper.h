// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_callback.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefCallbackWrapper : public ICallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefCallback> _callback;

        public:
            CefCallbackWrapper(CefRefPtr<CefCallback> &callback) :
                _callback(callback)
            {

            }

            !CefCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefCallbackWrapper()
            {
                this->!CefCallbackWrapper();

                _disposed = true;
            }

            virtual void Cancel()
            {
                ThrowIfDisposed();

                _callback->Cancel();

                delete this;
            }

            virtual void Continue()
            {
                ThrowIfDisposed();

                _callback->Continue();

                delete this;
            }
        };
    }
}
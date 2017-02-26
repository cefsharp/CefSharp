// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_geolocation_handler.h"

#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefGeolocationCallbackWrapper : public IGeolocationCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefGeolocationCallback> _callback;

        public:
            CefGeolocationCallbackWrapper(CefRefPtr<CefGeolocationCallback> &callback)
                : _callback(callback)
            {
            }

            !CefGeolocationCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefGeolocationCallbackWrapper()
            {
                this->!CefGeolocationCallbackWrapper();

                _disposed = true;
            }

            virtual void Continue(bool allow)
            {
                ThrowIfDisposed();

                _callback->Continue(allow);

                delete this;
            }
        };
    }
}


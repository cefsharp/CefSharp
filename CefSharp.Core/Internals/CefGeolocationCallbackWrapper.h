// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_geolocation_handler.h"

namespace CefSharp
{
    public ref class CefGeolocationCallbackWrapper : IGeolocationCallback
    {
    private:
        MCefRefPtr<CefGeolocationCallback> _callback;

    public:
        CefGeolocationCallbackWrapper(CefRefPtr<CefGeolocationCallback> &callback) : _callback(callback)
        {
            
        }

        ~CefGeolocationCallbackWrapper()
        {
            _callback = NULL;
        }

        virtual void Continue(bool allow)
        {
            _callback->Continue(allow);
        }
    };
}


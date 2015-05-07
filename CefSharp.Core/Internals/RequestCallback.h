// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "MCefRefPtr.h"

using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        public ref class RequestCallback : public IRequestCallback
        {
            MCefRefPtr<CefRequestCallback> _callback;
        internal:
            RequestCallback(CefRefPtr<CefRequestCallback> callback) : _callback(callback)
            {
            }

            ~RequestCallback()
            {
                _callback = NULL;
            }

        public:
            virtual void Continue(bool allow)
            {
                _callback->Continue(allow);
            }

            virtual void Cancel()
            {
                _callback->Cancel();
            }
        };
    }
}

// Copyright � 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefRequestCallbackWrapper : public IRequestCallback
        {
            MCefRefPtr<CefRequestCallback> _callback;
        internal:
            CefRequestCallbackWrapper(CefRefPtr<CefRequestCallback> callback) : _callback(callback)
            {
            }

            ~CefRequestCallbackWrapper()
            {
                _callback = NULL;
            }

        public:
            virtual void Continue(bool allow)
            {
                _callback->Continue(allow);

                _callback = NULL;
            }

            virtual void Cancel()
            {
                _callback->Cancel();

                _callback = NULL;
            }
        };
    }
}

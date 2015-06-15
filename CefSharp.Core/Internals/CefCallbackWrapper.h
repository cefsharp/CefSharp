// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_callback.h"

namespace CefSharp
{
    public ref class CefCallbackWrapper : ICallback
    {
    private:
        MCefRefPtr<CefCallback> _callback;

    public:
        CefCallbackWrapper(CefRefPtr<CefCallback> &callback) : _callback(callback)
        {
            
        }

        !CefCallbackWrapper()
        {
            _callback = NULL;
        }

        ~CefCallbackWrapper()
        {
            this->!CefCallbackWrapper();
        }

        virtual void Cancel()
        {
            _callback->Cancel();

            delete this;
        }

        virtual void Continue()
        {
            _callback->Continue();

            delete this;
        }
    };
}
// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_callback.h"

namespace CefSharp
{
    public ref class CefCallbackWrapper : public ICallback
    {
    private:
        MCefRefPtr<CefCallback> _callback;
        bool _disposed;

    public:
        CefCallbackWrapper(CefRefPtr<CefCallback> &callback) :
            _callback(callback),
            _disposed(false)
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
            if (_callback.get())
            {
                _callback->Cancel();

                delete this;
            }
        }

        virtual void Continue()
        {
            if (_callback.get())
            {
                _callback->Continue();

                delete this;
            }
        }

        virtual property bool IsDisposed
        {
            bool get()
            {
                return _disposed;
            }
        }
    };
}
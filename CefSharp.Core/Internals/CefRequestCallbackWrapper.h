// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
        private:
            MCefRefPtr<CefRequestCallback> _callback;
            IFrame^ _frame;
            IRequest^ _request;
            bool _disposed;

        internal:
            CefRequestCallbackWrapper(CefRefPtr<CefRequestCallback> &callback)
                : _callback(callback), _disposed(false)
            {
            }

            CefRequestCallbackWrapper(
                CefRefPtr<CefRequestCallback> &callback,
                IFrame^ frame,
                IRequest^ request)
                : _callback(callback), _frame(frame), _request(request)
            {
            }

            !CefRequestCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefRequestCallbackWrapper()
            {
                this->!CefRequestCallbackWrapper();
                delete _request;
                _request = nullptr;
                delete _frame;
                _frame = nullptr;

                _disposed = false;
            }

        public:
            virtual void Continue(bool allow)
            {
                _callback->Continue(allow);
                delete this;
            }

            virtual void Cancel()
            {
                _callback->Cancel();
                delete this;
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
}

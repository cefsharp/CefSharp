// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefJSDialogCallbackWrapper : public IJsDialogCallback, public CefWrapper
        {
            MCefRefPtr<CefJSDialogCallback> _callback;

        internal:
            CefJSDialogCallbackWrapper(CefRefPtr<CefJSDialogCallback> &callback)
                : _callback(callback)
            {
            }

            !CefJSDialogCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefJSDialogCallbackWrapper()
            {
                this->!CefJSDialogCallbackWrapper();

                _disposed = true;
            }

        public:
            virtual void Continue(bool success, String^ userInput)
            {
                ThrowIfDisposed();

                _callback->Continue(success, StringUtils::ToNative(userInput));

                delete this;
            }

            virtual void Continue(bool success)
            {
                ThrowIfDisposed();

                _callback->Continue(success, CefString());

                delete this;
            }
        };
    }
}

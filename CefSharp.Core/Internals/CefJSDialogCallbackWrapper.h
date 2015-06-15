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
        public ref class CefJSDialogCallbackWrapper : public IJsDialogCallback
        {
            MCefRefPtr<CefJSDialogCallback> _callback;

        internal:
            CefJSDialogCallbackWrapper(CefRefPtr<CefJSDialogCallback> &callback) : _callback(callback)
            {
            }

            !CefJSDialogCallbackWrapper()
            {
                _callback = NULL;
            }

            ~CefJSDialogCallbackWrapper()
            {
                this->!CefJSDialogCallbackWrapper();
            }

        public:
            virtual void Continue(bool success, String^ userInput)
            {
                _callback->Continue(success, StringUtils::ToNative(userInput));
                delete this;
            }

            virtual void Continue(bool success)
            {
                _callback->Continue(success, CefString());
                delete this;
            }
        };
    }
}

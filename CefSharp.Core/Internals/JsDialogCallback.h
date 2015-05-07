// Copyright � 2010-2015 The CefSharp Authors. All rights reserved.
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
        public ref class JsDialogCallback : public IJsDialogCallback
        {
            MCefRefPtr<CefJSDialogCallback> _callback;
        internal:
            JsDialogCallback(CefRefPtr<CefJSDialogCallback> callback) : _callback(callback)
            {
            }

            ~JsDialogCallback()
            {
                _callback = NULL;
            }

        public:
            virtual void Continue(bool success, String^ userInput)
            {
                _callback->Continue(success, StringUtils::ToNative(userInput));
            }
        };
    }
}

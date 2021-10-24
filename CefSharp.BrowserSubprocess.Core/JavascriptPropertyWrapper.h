// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

using namespace CefSharp::Internals::Wcf;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private ref class JavascriptPropertyWrapper
        {
        private:
            int64 _ownerId;
            IBrowserProcess^ _browserProcess;
            //TODO: Strongly type this variable - currently trying to include JavascriptObjectWrapper.h creates a circular reference, so won't compile
            Object^ _javascriptObjectWrapper;

        public:
            JavascriptPropertyWrapper(int64 ownerId, IBrowserProcess^ browserProcess)
            {
                _ownerId = ownerId;
                _browserProcess = browserProcess;
                _javascriptObjectWrapper = nullptr;
            }

            ~JavascriptPropertyWrapper()
            {
                if (_javascriptObjectWrapper != nullptr)
                {
                    delete _javascriptObjectWrapper;
                    _javascriptObjectWrapper = nullptr;
                }
            }

            void Bind(JavascriptProperty^ javascriptProperty, const CefRefPtr<CefV8Value>& v8Value, JavascriptCallbackRegistry^ callbackRegistry);
        };
    }
}

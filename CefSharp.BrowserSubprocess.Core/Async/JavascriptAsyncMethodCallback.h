// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        namespace Async
        {
            private ref class JavascriptAsyncMethodCallback
            {
            private:
                MCefRefPtr<CefV8Context> _context;
                MCefRefPtr<CefV8Value> _promise;

            public:
                JavascriptAsyncMethodCallback(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> promise)
                    :_context(context), _promise(promise.get())
                {

                }

                !JavascriptAsyncMethodCallback()
                {
                    _context = nullptr;
                    _promise = nullptr;
                }

                ~JavascriptAsyncMethodCallback()
                {
                    this->!JavascriptAsyncMethodCallback();
                }

                void Success(const CefRefPtr<CefV8Value>& result);

                void Fail(const CefString& exception);
            };
        }
    }
}

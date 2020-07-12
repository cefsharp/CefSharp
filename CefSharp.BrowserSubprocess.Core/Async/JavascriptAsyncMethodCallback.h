// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncMethodCallback
            {
            private:
                MCefRefPtr<CefV8Context> _context;
                MCefRefPtr<CefV8Value> _resolve;
                MCefRefPtr<CefV8Value> _reject;

            public:
                JavascriptAsyncMethodCallback(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> resolve, CefRefPtr<CefV8Value> reject)
                    :_context(context), _resolve(resolve.get()), _reject(reject.get())
                {

                }

                !JavascriptAsyncMethodCallback()
                {
                    _resolve = nullptr;
                    _reject = nullptr;
                    _context = nullptr;
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
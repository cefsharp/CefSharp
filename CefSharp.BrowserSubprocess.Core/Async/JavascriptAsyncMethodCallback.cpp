// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodCallback.h"

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        namespace Async
        {
            void JavascriptAsyncMethodCallback::Success(const CefRefPtr<CefV8Value>& result)
            {
                if (_promise.get() && _context.get() && _context->Enter())
                {
                    try
                    {
                        _promise->ResolvePromise(result);
                    }
                    finally
                    {
                        _context->Exit();
                    }
                }
            }

            void JavascriptAsyncMethodCallback::Fail(const CefString& exception)
            {
                if (_promise.get() && _context.get() && _context->Enter())
                {
                    try
                    {
                        _promise->RejectPromise(exception);
                    }
                    finally
                    {
                        _context->Exit();
                    }
                }
            }
        }
    }
}

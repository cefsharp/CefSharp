// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptAsyncMethodCallback.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            void JavascriptAsyncMethodCallback::Success(const CefRefPtr<CefV8Value>& result)
            {
                if (_resolve.get() && _context.get() && _context->Enter())
                {
                    try
                    {
                        CefV8ValueList args;
                        args.push_back(result);
                        _resolve->ExecuteFunction(nullptr, args);
                    }
                    finally
                    {
                        _context->Exit();
                    }
                }
            }

            void JavascriptAsyncMethodCallback::Fail(const CefString& exception)
            {
                if (_reject.get() && _context.get() && _context->Enter())
                {
                    try
                    {
                        CefV8ValueList args;
                        args.push_back(CefV8Value::CreateString(exception));
                        _reject->ExecuteFunction(nullptr, args);
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

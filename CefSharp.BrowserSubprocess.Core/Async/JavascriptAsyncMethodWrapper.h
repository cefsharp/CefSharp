// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptAsyncMethodHandler.h"
#include "JavascriptCallbackRegistry.h"

namespace CefSharp
{
    ref class CefBrowserWrapper;

    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncMethodWrapper
            {
            private:
                MCefRefPtr<JavascriptAsyncMethodHandler> _javascriptMethodHandler;
                JavascriptMethod^ _method;

            public:
                JavascriptAsyncMethodWrapper(CefBrowserWrapper^ browser, JavascriptMethod^ method, int64 ownerId, JavascriptCallbackRegistry^ callbackRegistry)
                    :_method(method), _javascriptMethodHandler(new JavascriptAsyncMethodHandler(method, ownerId, browser, callbackRegistry))
                {

                }

                void Bind(const CefRefPtr<CefV8Value>& value);
            };
        }
    }
}
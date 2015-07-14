// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

namespace CefSharp
{
    ref class CefBrowserWrapper;

    namespace Internals
    {
        namespace Async
        {
            class JavascriptAsyncMethodHandler : public virtual CefV8Handler
            {
            private:
                gcroot<JavascriptCallbackRegistry^> _callbackRegistry;
                gcroot<CefBrowserWrapper^> _browser;
                gcroot<JavascriptMethod^> _method;
                int64 _objectId;

            public:
                JavascriptAsyncMethodHandler(JavascriptMethod^ method, int64 objectId, CefBrowserWrapper^ browser, JavascriptCallbackRegistry^ callbackRegistry)
                    :_callbackRegistry(callbackRegistry), _browser(browser), _method(method), _objectId(objectId)
                {

                }

                virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);

                IMPLEMENT_REFCOUNTING(JavascriptAsyncMethodHandler)
            };
        }
    }
}

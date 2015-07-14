// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncMethodWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncObjectWrapper
            {
            private:
                initonly List<JavascriptAsyncMethodWrapper^>^ _wrappedMethods;
                CefBrowserWrapper^ _browser;
                JavascriptObject^ _object;

            internal:
                JavascriptCallbackRegistry^ CallbackRegistry;

            public:
                JavascriptAsyncObjectWrapper(CefBrowserWrapper^ browser, JavascriptObject^ object)
                    :_object(object), _browser(browser), _wrappedMethods(gcnew List<JavascriptAsyncMethodWrapper^>())
                {

                }

                ~JavascriptAsyncObjectWrapper()
                {
                    CallbackRegistry = nullptr;

                    for each (JavascriptAsyncMethodWrapper^ var in _wrappedMethods)
                    {
                        delete var;
                    }
                }

                void Bind(const CefRefPtr<CefV8Value> &value);
            };
        }
    }
}
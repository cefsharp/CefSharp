// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncObjectWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    ref class CefBrowserWrapper;

    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncRootObjectWrapper
            {
            private:
                initonly List<JavascriptAsyncObjectWrapper^>^ _wrappedObjects;
                CefBrowserWrapper^ _browser;
                JavascriptRootObject^ _rootObject;

            internal:
                MCefRefPtr<CefV8Value> V8Value;

                // The entire set of possible JavaScript functions to
                // call directly into.
                JavascriptCallbackRegistry^ CallbackRegistry;

            public:
                JavascriptAsyncRootObjectWrapper(CefBrowserWrapper^ browser, JavascriptRootObject^ rootObject)
                    :_rootObject(rootObject), _browser(browser), _wrappedObjects(gcnew List<JavascriptAsyncObjectWrapper^>())
                {

                }

                !JavascriptAsyncRootObjectWrapper()
                {
                    V8Value = nullptr;
                }

                ~JavascriptAsyncRootObjectWrapper()
                {
                    this->!JavascriptAsyncRootObjectWrapper();
                    CallbackRegistry = nullptr;
                    for each (JavascriptAsyncObjectWrapper^ var in _wrappedObjects)
                    {
                        delete var;
                    }
                }

                void Bind();
            };
        }
    }
}

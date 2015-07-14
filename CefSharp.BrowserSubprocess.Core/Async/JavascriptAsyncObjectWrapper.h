// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptAsyncMethodCallback.h"
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
                JavascriptObject^ _object;

            internal:
                Func<JavascriptAsyncMethodCallback^, int64>^ MethodCallbackSave;
                JavascriptCallbackRegistry^ CallbackRegistry;
                MCefRefPtr<CefV8Value> PromiseCreator;

            public:
                JavascriptAsyncObjectWrapper(JavascriptObject^ object)
                    :_object(object), _wrappedMethods(gcnew List<JavascriptAsyncMethodWrapper^>())
                {

                }

                !JavascriptAsyncObjectWrapper()
                {
                    PromiseCreator = nullptr;
                }

                ~JavascriptAsyncObjectWrapper()
                {
                    this->!JavascriptAsyncObjectWrapper();
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
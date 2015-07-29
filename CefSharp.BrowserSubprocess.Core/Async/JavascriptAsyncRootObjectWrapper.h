// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptAsyncMethodCallback.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncObjectWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            private ref class JavascriptAsyncRootObjectWrapper
            {
            private:
                initonly List<JavascriptAsyncObjectWrapper^>^ _wrappedObjects;
				// The entire set of possible JavaScript functions to
				// call directly into.
				JavascriptCallbackRegistry^ _callbackRegistry;
				Func<JavascriptAsyncMethodCallback^, int64>^ _methodCallbackSave;
                JavascriptRootObject^ _rootObject;

            public:
				JavascriptAsyncRootObjectWrapper(JavascriptRootObject^ rootObject, JavascriptCallbackRegistry^ callbackRegistry, Func<JavascriptAsyncMethodCallback^, int64>^ saveMethod)
					:_rootObject(rootObject), _wrappedObjects(gcnew List<JavascriptAsyncObjectWrapper^>()), _callbackRegistry(callbackRegistry), _methodCallbackSave(saveMethod)
                {

                }

                ~JavascriptAsyncRootObjectWrapper()
                {
					_methodCallbackSave = nullptr;
                    _callbackRegistry = nullptr;
                    for each (JavascriptAsyncObjectWrapper^ var in _wrappedObjects)
                    {
                        delete var;
                    }
                }

				void Bind(const CefRefPtr<CefV8Value>& v8Value);
            };
        }
    }
}

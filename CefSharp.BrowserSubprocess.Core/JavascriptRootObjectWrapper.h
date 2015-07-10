// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptObjectWrapper.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals;

namespace CefSharp
{
    // This wraps the transmitted registered objects
    // by binding the meta-data to V8 JavaScript objects
    // and installing callbacks for changes to those
    // objects.
    public ref class JavascriptRootObjectWrapper
    {
    private:
        JavascriptRootObject^ _rootObject;
        List<JavascriptObjectWrapper^>^ _wrappedObjects;
        IBrowserProcess^ _browserProcess;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

        // The entire set of possible JavaScript functions to
        // call directly into.
        JavascriptCallbackRegistry^ CallbackRegistry;

    public:
        JavascriptRootObjectWrapper(JavascriptRootObject^ rootObject, IBrowserProcess^ browserProcess)
        {
            _rootObject = rootObject;
            _browserProcess = browserProcess;
            _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
        }

        !JavascriptRootObjectWrapper()
        {
            V8Value = nullptr;
        }

        ~JavascriptRootObjectWrapper()
        {
            this->!JavascriptRootObjectWrapper();
            delete CallbackRegistry;
            CallbackRegistry = nullptr;
            for each (JavascriptObjectWrapper^ var in _wrappedObjects)
            {
                delete var;
            }
        }

        void Bind();
    };
}


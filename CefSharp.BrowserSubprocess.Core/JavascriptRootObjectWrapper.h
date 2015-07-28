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
        // The entire set of possible JavaScript functions to
        // call directly into.
        JavascriptCallbackRegistry^ _callbackRegistry;

    internal:
        property JavascriptCallbackRegistry^ CallbackRegistry
        {
            CefSharp::Internals::JavascriptCallbackRegistry^ get();
        }

    public:
        JavascriptRootObjectWrapper(int browserId, JavascriptRootObject^ rootObject, IBrowserProcess^ browserProcess)
        {
            _rootObject = rootObject;
            _browserProcess = browserProcess;
            _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
            _callbackRegistry = gcnew JavascriptCallbackRegistry(browserId);
        }

        ~JavascriptRootObjectWrapper()
        {
            if (_callbackRegistry != nullptr)
            {
                delete _callbackRegistry;
                _callbackRegistry = nullptr;
            }

            for each (JavascriptObjectWrapper^ var in _wrappedObjects)
            {
                delete var;
            }
        }

        void Bind(const CefRefPtr<CefV8Value>& v8Value);
    };
}


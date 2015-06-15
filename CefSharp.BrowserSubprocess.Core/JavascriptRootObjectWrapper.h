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
        // TODO: Is this member variable necessary?
        // We only use it to call a static method on CefV8Value atm.
        MCefRefPtr<CefV8Value> V8Value;

        // The entire set of possible JavaScript functions to
        // call directly into.
        MCefRefPtr<JavascriptCallbackRegistry> CallbackRegistry;

    public:
        JavascriptRootObjectWrapper(JavascriptRootObject^ rootObject, IBrowserProcess^ browserProcess);

        !JavascriptRootObjectWrapper();
        ~JavascriptRootObjectWrapper();

        void Bind();
    };
}


// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptObjectWrapper.h"
#include "Async/JavascriptAsyncObjectWrapper.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals::Async;

namespace CefSharp
{
    ref class CefBrowserWrapper;

    // This wraps the transmitted registered objects
    // by binding the meta-data to V8 JavaScript objects
    // and installing callbacks for changes to those
    // objects.
    private ref class JavascriptRootObjectWrapper
    {
    private:
        initonly List<JavascriptObjectWrapper^>^ _wrappedObjects;
        initonly List<JavascriptAsyncObjectWrapper^>^ _wrappedAsyncObjects;
        IBrowserProcess^ _browserProcess;

    public:
        JavascriptRootObjectWrapper(int browserId, IBrowserProcess^ browserProcess)
        {
            _browserProcess = browserProcess;
            _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
            _wrappedAsyncObjects = gcnew List<JavascriptAsyncObjectWrapper^>();
        }

        ~JavascriptRootObjectWrapper()
        {
            for each (JavascriptObjectWrapper^ var in _wrappedObjects)
            {
                delete var;
            }
            _wrappedObjects->Clear();

            for each (JavascriptAsyncObjectWrapper^ var in _wrappedAsyncObjects)
            {
                delete var;
            }
            _wrappedAsyncObjects->Clear();
        }

        void Bind(CefBrowserWrapper^ browserWrapper, JavascriptRootObject^ rootObject, JavascriptRootObject^ asyncRootObject, const CefRefPtr<CefV8Value>& v8Value);
    };
}


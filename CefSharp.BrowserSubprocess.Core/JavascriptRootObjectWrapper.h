// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"
#ifndef NETCOREAPP
#include "JavascriptObjectWrapper.h"
#endif
#include "Async/JavascriptAsyncObjectWrapper.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals::Async;
#ifndef NETCOREAPP
using namespace CefSharp::Internals::Wcf;
#endif

namespace CefSharp
{
    // This wraps the transmitted registered objects
    // by binding the meta-data to V8 JavaScript objects
    // and installing callbacks for changes to those
    // objects.
    private ref class JavascriptRootObjectWrapper
    {
    private:
        //Only access through Interlocked::Increment - used to generate unique callback Id's
        //Is static so ids are unique to this process https://github.com/cefsharp/CefSharp/issues/2792
        static int64 _lastCallback;

#ifndef NETCOREAPP
        initonly List<JavascriptObjectWrapper^>^ _wrappedObjects;
#endif
        initonly List<JavascriptAsyncObjectWrapper^>^ _wrappedAsyncObjects;
        initonly Dictionary<int64, JavascriptAsyncMethodCallback^>^ _methodCallbacks;
#ifndef NETCOREAPP
        IBrowserProcess^ _browserProcess;
#endif
        // The entire set of possible JavaScript functions to
        // call directly into.
        JavascriptCallbackRegistry^ _callbackRegistry;

        int64 SaveMethodCallback(JavascriptAsyncMethodCallback^ callback);

    internal:
        property JavascriptCallbackRegistry^ CallbackRegistry
        {
            CefSharp::Internals::JavascriptCallbackRegistry^ get();
        }

    public:
#ifdef NETCOREAPP
        JavascriptRootObjectWrapper(int browserId)
#else
        JavascriptRootObjectWrapper(int browserId, IBrowserProcess^ browserProcess)
#endif
        {
#ifndef NETCOREAPP
            _browserProcess = browserProcess;
            _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
#endif
            _wrappedAsyncObjects = gcnew List<JavascriptAsyncObjectWrapper^>();
            _callbackRegistry = gcnew JavascriptCallbackRegistry(browserId);
            _methodCallbacks = gcnew Dictionary<int64, JavascriptAsyncMethodCallback^>();
        }

        ~JavascriptRootObjectWrapper()
        {
            if (_callbackRegistry != nullptr)
            {
                delete _callbackRegistry;
                _callbackRegistry = nullptr;
            }

#ifndef NETCOREAPP
            for each (JavascriptObjectWrapper^ var in _wrappedObjects)
            {
                delete var;
            }
            _wrappedObjects->Clear();

#endif

            for each (JavascriptAsyncObjectWrapper^ var in _wrappedAsyncObjects)
            {
                delete var;
            }
            _wrappedAsyncObjects->Clear();

            for each(JavascriptAsyncMethodCallback^ var in _methodCallbacks->Values)
            {
                delete var;
            }
            _methodCallbacks->Clear();
        }

        bool TryGetAndRemoveMethodCallback(int64 id, JavascriptAsyncMethodCallback^% callback);

        void Bind(ICollection<JavascriptObject^>^ objects, const CefRefPtr<CefV8Value>& v8Value);
    };
}


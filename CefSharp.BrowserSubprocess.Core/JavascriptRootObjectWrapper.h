// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptObjectWrapper.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptRootObjectWrapper
    {
    private:
        JavascriptRootObject^ _rootObject;
        List<JavascriptObjectWrapper^>^ _wrappedObjects;
        IBrowserProcess^ _browserProcess;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptRootObjectWrapper(JavascriptRootObject^ rootObject, IBrowserProcess^ browserProcess)
        {
            _rootObject = rootObject;
            _browserProcess = browserProcess;
            _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
        }

        ~JavascriptRootObjectWrapper()
        {
            V8Value = nullptr;
            for each (JavascriptObjectWrapper^ var in _wrappedObjects)
            {
                delete var;
            }
        }

        void Bind()
        {
            auto memberObjects = _rootObject->MemberObjects;
            for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
            {
                auto wrapperObject = gcnew JavascriptObjectWrapper(obj, _browserProcess);
                wrapperObject->V8Value = V8Value.get();
                wrapperObject->Bind();

                _wrappedObjects->Add(wrapperObject);
            }
        };
    };
}


// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptMethodWrapper.h"
#include "JavascriptPropertyWrapper.h"
#include "JavascriptPropertyHandler.h"

using namespace System::Runtime::Serialization;
using namespace System::Linq;
using namespace System::Collections::Generic;

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptObjectWrapper
    {
    private:
        JavascriptObject^ _object;
        List<JavascriptMethodWrapper^>^ _wrappedMethods;
        List<JavascriptPropertyWrapper^>^ _wrappedProperties;
        IBrowserProcess^ _browserProcess;
        MCefRefPtr<JavascriptPropertyHandler> _jsPropertyHandler;

    internal:
        MCefRefPtr<CefV8Value> V8Value;
        

    public:
        JavascriptObjectWrapper(JavascriptObject^ object, IBrowserProcess^ browserProcess)
        {
            _object = object;
            _browserProcess = browserProcess;

            _wrappedMethods = gcnew List<JavascriptMethodWrapper^>();
            _wrappedProperties = gcnew List<JavascriptPropertyWrapper^>();
        }

        ~JavascriptObjectWrapper()
        {
            V8Value = nullptr;
            _jsPropertyHandler->Cleanup();
            _jsPropertyHandler = nullptr;

            for each (JavascriptMethodWrapper^ var in _wrappedMethods)
            {
                delete var;
            }
            for each (JavascriptPropertyWrapper^ var in _wrappedProperties)
            {
                delete var;
            }
        }

        void Bind();
        BrowserProcessResponse^ GetProperty(String^ memberName);
        BrowserProcessResponse^ SetProperty(String^ memberName, Object^ value);
    };
}
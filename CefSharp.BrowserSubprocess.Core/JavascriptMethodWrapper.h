// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptMethodHandler.h"

using namespace CefSharp::Internals;
using namespace System::Runtime::Serialization;

namespace CefSharp
{
    public ref class JavascriptMethodWrapper
    {
    private:
        MCefRefPtr<JavascriptMethodHandler> _javascriptMethodHandler;
        JavascriptMethod^ _javascriptMethod;
        int64 _ownerId;
        IBrowserProcess^ _browserProcess;

    public:
        JavascriptMethodWrapper(JavascriptMethod^ javascriptMethod, int64 ownerId, IBrowserProcess^ browserProcess, JavascriptCallbackRegistry^ callbackRegistry)
        {
            _javascriptMethod = javascriptMethod;
            _ownerId = ownerId;
            _browserProcess = browserProcess;
            _javascriptMethodHandler = new JavascriptMethodHandler(gcnew Func<array<Object^>^, BrowserProcessResponse^>(this, &JavascriptMethodWrapper::Execute), callbackRegistry);
        }

        !JavascriptMethodWrapper()
        {
            _javascriptMethodHandler = nullptr;
        }

        ~JavascriptMethodWrapper()
        {
            this->!JavascriptMethodWrapper();

            _javascriptMethod = nullptr;
            _browserProcess = nullptr;
        }

        void Bind(const CefRefPtr<CefV8Value>& v8Value);
        BrowserProcessResponse^ Execute(array<Object^>^ parameters);
    };
}
// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
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

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptMethodWrapper(JavascriptMethod^ javascriptMethod, int64 ownerId, IBrowserProcess^ browserProcess)
        {
            _javascriptMethod = javascriptMethod;
            _ownerId = ownerId;
            _browserProcess = browserProcess;
            _javascriptMethodHandler = new JavascriptMethodHandler(gcnew Func<array<Object^>^, BrowserProcessResponse^>(this, &JavascriptMethodWrapper::Execute));
        }

        ~JavascriptMethodWrapper()
        {
            V8Value = nullptr;
            _javascriptMethodHandler = nullptr;
        }

        void Bind();
        BrowserProcessResponse^ Execute(array<Object^>^ parameters);
    };
}
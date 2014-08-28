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
        long _ownerId;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptMethodWrapper(JavascriptMethod^ javascriptMethod, long ownerId)
        {
            _javascriptMethod = javascriptMethod;
            _ownerId = ownerId;
            _javascriptMethodHandler = new JavascriptMethodHandler(gcnew Func<array<Object^>^, Object^>(this, &JavascriptMethodWrapper::Execute));
        }

        void Bind();
        Object^ Execute(array<Object^>^ parameters);
    };
}
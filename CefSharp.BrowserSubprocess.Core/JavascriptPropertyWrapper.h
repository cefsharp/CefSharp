// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"

#include "JavascriptPropertyHandler.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptPropertyWrapper
    {
    private:
        JavascriptProperty^ _javascriptProperty;
        int64 _ownerId;
        IBrowserProcess^ _browserProcess;

    internal:
        MCefRefPtr<CefV8Value> V8Value;

    public:
        JavascriptPropertyWrapper(JavascriptProperty^ javascriptProperty, int64 ownerId, IBrowserProcess^ browserProcess)
        {
            _javascriptProperty = javascriptProperty;
            _ownerId = ownerId;
            _browserProcess = browserProcess;
        }

        ~JavascriptPropertyWrapper()
        {
            V8Value = nullptr;
        }

        void Bind();
    };
}
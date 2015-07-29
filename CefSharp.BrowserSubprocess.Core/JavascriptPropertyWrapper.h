﻿// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class JavascriptPropertyWrapper
    {
    private:
        JavascriptProperty^ _javascriptProperty;
        int64 _ownerId;
        IBrowserProcess^ _browserProcess;
        //TODO: Strongly type this variable - currently trying to include JavascriptObjectWrapper.h creates a circular reference, so won't compile
        Object^ _javascriptObjectWrapper;

    public:
        JavascriptPropertyWrapper(JavascriptProperty^ javascriptProperty, int64 ownerId, IBrowserProcess^ browserProcess)
        {
            _javascriptProperty = javascriptProperty;
            _ownerId = ownerId;
            _browserProcess = browserProcess;
        }

        ~JavascriptPropertyWrapper()
        {
            if (_javascriptObjectWrapper != nullptr)
            {
                delete _javascriptObjectWrapper;
                _javascriptObjectWrapper = nullptr;
            }
            _javascriptProperty = nullptr;
        }

        void Bind(const CefRefPtr<CefV8Value>& v8Value, JavascriptCallbackRegistry^ callbackRegistry);
    };
}
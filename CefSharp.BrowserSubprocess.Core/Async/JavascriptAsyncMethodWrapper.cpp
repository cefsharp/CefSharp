// Copyright � 2010-2016 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"
#include "JavascriptAsyncMethodWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            void JavascriptAsyncMethodWrapper::Bind(JavascriptMethod^ method, const CefRefPtr<CefV8Value>& value)
            {
                auto methodName = StringUtils::ToNative(method->JavascriptName);
                auto v8Function = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

                value->SetValue(methodName, v8Function, V8_PROPERTY_ATTRIBUTE_NONE);
            }
        }
    }
}
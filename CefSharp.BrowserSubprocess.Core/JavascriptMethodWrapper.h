// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptObjectWrapper.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private ref class JavascriptMethodWrapper : public JavascriptMember
    {
    private:
        MCefRefPtr<JavascriptMethodHandler> _javascriptMethodHandler;

    public:

        JavascriptMethodWrapper()
        {
            _javascriptMethodHandler = new JavascriptMethodHandler();
        }

        virtual void Bind(JavascriptObject^ owner) override
        {
            auto realOwner = static_cast<JavascriptObjectWrapper^>(owner);
            
            auto methodName = StringUtils::ToNative(Description->JavascriptName);
            auto v8Value = CefV8Value::CreateFunction(methodName, _javascriptMethodHandler.get());

            realOwner->Value->SetValue(methodName, v8Value, V8_PROPERTY_ATTRIBUTE_NONE);
        };
    };
}
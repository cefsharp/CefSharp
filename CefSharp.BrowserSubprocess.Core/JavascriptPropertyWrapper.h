// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptObjectWrapper.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private ref class JavascriptPropertyWrapper : public JavascriptProperty
    {    
    private:
        MCefRefPtr<JavascriptPropertyHandler> _javascriptPropertyHandler;
        JavascriptObjectWrapper^ _owner;
        
    public:
        
        JavascriptPropertyWrapper()
        {
            _javascriptPropertyHandler = new JavascriptPropertyHandler();
        }
        
        virtual void Bind(JavascriptObject^ owner) override
        {
            _owner = static_cast<JavascriptObjectWrapper^>(owner);
            auto realValue = static_cast<JavascriptObjectWrapper^>(Value);
            auto v8Value = _owner->Value->CreateObject(_javascriptPropertyHandler.get());
            
            realValue->Value = v8Value;

            _owner->Value->SetValue(StringUtils::ToNative(Description->JavascriptName), v8Value, V8_PROPERTY_ATTRIBUTE_NONE);

            realValue->Bind();
        };
    };
}
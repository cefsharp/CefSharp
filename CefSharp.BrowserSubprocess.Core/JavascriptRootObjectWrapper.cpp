// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"

namespace CefSharp
{
    JavascriptRootObjectWrapper::JavascriptRootObjectWrapper(JavascriptRootObject^ rootObject, IBrowserProcess^ browserProcess)
    {
        _rootObject = rootObject;
        _browserProcess = browserProcess;
        _wrappedObjects = gcnew List<JavascriptObjectWrapper^>();
    }

    JavascriptRootObjectWrapper::~JavascriptRootObjectWrapper()
    {
        V8Value = nullptr;
        CallbackRegistry = nullptr;
        for each (JavascriptObjectWrapper^ var in _wrappedObjects)
        {
            delete var;
        }
    }

    void JavascriptRootObjectWrapper::Bind()
    {
        auto memberObjects = _rootObject->MemberObjects;
        for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
        {
            auto wrapperObject = gcnew JavascriptObjectWrapper(obj, _browserProcess);
            wrapperObject->CallbackRegistry = CallbackRegistry;
            wrapperObject->V8Value = V8Value.get();
            wrapperObject->Bind();

            _wrappedObjects->Add(wrapperObject);
        }
    }
}
// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"

namespace CefSharp
{
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
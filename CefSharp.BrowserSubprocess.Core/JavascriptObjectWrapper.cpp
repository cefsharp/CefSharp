// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"
#include "JavascriptPropertyWrapper.h"
#include "JavascriptMethodWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptObjectWrapper::Clone(JavascriptObject^ obj)
    {
        Id = obj->Id;

        for each (JavascriptProperty^ prop in Enumerable::OfType<JavascriptProperty^>(obj->Properties))
        {
            auto propertywrapper = gcnew JavascriptPropertyWrapper();
            propertywrapper->Clone(prop);
            Properties->Add(propertywrapper);
        }

        for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(obj->Methods))
        {
            auto methodwrapper = gcnew JavascriptMethodWrapper();
            methodwrapper->Clone(method);
            Methods->Add(methodwrapper);
        }
    };
}
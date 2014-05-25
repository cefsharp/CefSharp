// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptMethodWrapper.h"
#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
    Object^ JavascriptMethodWrapper::Execute(array<Object^>^ parameters)
    {
        auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

        return browserProxy->CallMethod(_owner->Id, Description->ManagedName, parameters);
    }
}
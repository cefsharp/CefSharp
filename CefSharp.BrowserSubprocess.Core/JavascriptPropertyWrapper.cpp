// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptPropertyWrapper.h"
#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
    void JavascriptPropertyWrapper::SetProperty(String^ memberName, Object^ value)
    {
        auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

        browserProxy->SetProperty(_ownerId, memberName, value);
    };

    Object^ JavascriptPropertyWrapper::GetProperty(String^ memberName)
    {
        auto browserProxy = CefAppWrapper::Instance->CreateBrowserProxy();

        return browserProxy->GetProperty(_ownerId, memberName);
    };
}
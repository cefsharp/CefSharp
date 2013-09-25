// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "SchemeHandler.h"

using namespace System;

namespace CefSharp
{
    public ref class CefCustomScheme
    {
        property String^ SchemeName;
        property String^ DomainName;
        property bool IsStandard;
        property ISchemeHandlerFactory ^SchemeHandlerFactory;
    };
}

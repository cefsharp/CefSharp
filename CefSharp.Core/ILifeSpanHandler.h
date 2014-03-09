// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;

namespace CefSharp
{
    public interface class ILifeSpanHandler
    {
        bool OnBeforePopup(IWebBrowser^ browser, String^ url, int% x, int% y, int% width, int% height);
        void OnBeforeClose(IWebBrowser^ browser);
    };
}
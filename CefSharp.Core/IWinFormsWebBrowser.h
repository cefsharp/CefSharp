// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    // Should rightfulyl live in the CefSharp.WinForms project, but the problem is that it's being used from the CefSharp project
    // so the dependency would go the wrong way... Has to be here for the time being.
    public interface class IWinFormsWebBrowser : IWebBrowser
    {
        property IMenuHandler^ MenuHandler;
    };
}

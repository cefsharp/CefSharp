// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefAppWrapper.h"

using namespace System;

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper(Action<CefBrowserWrapper^>^ onBrowserCreated)
    {
        cefApp = new CefRefPtr<CefAppUnmanagedWrapper>(new CefAppUnmanagedWrapper(onBrowserCreated));
    }

    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, *(CefRefPtr<CefApp>*)cefApp);
    }
}
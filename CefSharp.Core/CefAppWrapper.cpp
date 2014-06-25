// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Wrappers\CefAppWrapper.h"

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper(CefSubprocess^ managedApp)
    {
        _managedApp = managedApp;
        cefApp = new CefAppUnmanagedWrapper(this);
    }

    int CefAppWrapper::Run(array<String^>^ args)
    {
        _managedApp->FindParentProcessId(args);

        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, (CefApp*)cefApp.get());
    }
}
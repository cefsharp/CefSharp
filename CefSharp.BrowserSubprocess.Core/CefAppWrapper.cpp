// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefAppWrapper.h"
#include "CefBrowserWrapper.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper()
    {
        auto onBrowserCreated = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserCreated);
        auto onBrowserDestroyed = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserDestroyed);
        cefApp = new CefAppUnmanagedWrapper(onBrowserCreated, onBrowserDestroyed);
        browserWrappers = gcnew Dictionary<int, CefBrowserWrapper^>();
    }

    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());
        Instance = this;

        return CefExecuteProcess(cefMainArgs, (CefApp*)cefApp.get());
    }
    
    void CefAppWrapper::Bind(JavascriptRootObject^ rootObject)
    {
        auto app = cefApp.get();
        
        app->Bind(rootObject);
    }
}
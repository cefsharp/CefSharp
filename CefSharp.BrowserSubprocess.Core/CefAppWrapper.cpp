// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefAppWrapper.h"
#include "CefSubprocessWrapper.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper()
    {
        Action<CefBrowserBase^>^ onBrowserCreated = gcnew Action<CefBrowserBase^>(this, &ManagedCefApp::OnBrowserCreated);
        cefApp = new CefAppUnmanagedWrapper(onBrowserCreated);
        browserWrappers = gcnew List<CefBrowserWrapper^>();
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
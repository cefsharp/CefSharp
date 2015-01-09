// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefAppWrapper.h"
#include "CefBrowserWrapper.h"
#include "CefTaskScheduler.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper()
    {
        auto onBrowserCreated = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserCreated);
        auto onBrowserDestroyed = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserDestroyed);
        _cefApp = new CefAppUnmanagedWrapper(onBrowserCreated, onBrowserDestroyed);

        RenderThreadTaskFactory = gcnew TaskFactory(gcnew CefTaskScheduler(TID_RENDERER));
    };

    CefAppWrapper::~CefAppWrapper()
    {
        RenderThreadTaskFactory = nullptr;
        _cefApp = nullptr;
    }

    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), NULL);
    }
}
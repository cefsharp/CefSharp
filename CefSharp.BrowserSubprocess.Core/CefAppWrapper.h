﻿// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"
#include "include/cef_base.h"

#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    // Wrap CefAppUnmangedWrapper in a nice managed wrapper
    public ref class CefAppWrapper abstract : public DisposableResource
    {
    private:
        MCefRefPtr<CefAppUnmanagedWrapper> _cefApp;
        
    public:        
        CefAppWrapper()
        {
            auto onBrowserCreated = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserCreated);
            auto onBrowserDestroyed = gcnew Action<CefBrowserWrapper^>(this, &CefAppWrapper::OnBrowserDestroyed);
            _cefApp = new CefAppUnmanagedWrapper(onBrowserCreated, onBrowserDestroyed);
        };

        !CefAppWrapper()
        {
            _cefApp = nullptr;
        }

        ~CefAppWrapper()
        {
            this->!CefAppWrapper();
        }

        int Run();

        virtual void OnBrowserCreated(CefBrowserWrapper^ cefBrowserWrapper) abstract;
        virtual void OnBrowserDestroyed(CefBrowserWrapper^ cefBrowserWrapper) abstract;		
    };
}

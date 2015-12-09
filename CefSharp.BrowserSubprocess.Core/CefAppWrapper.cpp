// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefAppWrapper.h"

using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace CefSharp
{
    bool CefAppWrapper::ArgumentExists(String^ argumentName, IEnumerable<String^>^ args)
    {
        for each(auto arg in args)
        {
            // arg can be either "--name=value" or "--name"
            auto parts = arg->Split('=');
            if (0 < parts->Length)
            {
                if (parts[0] == argumentName)
                {
                    return true;
                }
            }
        }

        return false;
    }

    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), NULL);
    }
}
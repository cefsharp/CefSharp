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
    List<String^>^ CefAppWrapper::ParseOptionalMessagesArgument(IEnumerable<String^>^ args)
    {
        auto optionalMessages = gcnew List<String^>();
        for each(auto arg in args)
        {
            if (arg->StartsWith(CefSharpArguments::OptionalMessagesArgument))
            {
                auto argValue = arg->Substring(CefSharpArguments::OptionalMessagesArgument->Length + 1);
                if (0 < argValue->Length)
                {
                    for each (auto entry in argValue->Split(','))
                    {
                        optionalMessages->Add(entry);
                    }
                }
            }
        }

        return optionalMessages;
    }

    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), NULL);
    }
}
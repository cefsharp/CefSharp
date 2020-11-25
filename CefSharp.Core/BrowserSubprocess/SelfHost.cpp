// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "SelfHost.h"

using namespace System;
using namespace System::IO;
using namespace CefSharp;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        int SelfHost::Main(array<String^>^ args)
        {
            auto type = CommandLineArgsParser::GetArgumentValue(args, CefSharpArguments::SubProcessTypeArgument);

            if (String::IsNullOrEmpty(type))
            {
                //If --type param missing from command line CEF/Chromium assums
                //this is the main process (as all subprocesses must have a type param).
                //Return -1 to indicate this behaviour.
                return -1;
            }

            auto browserSubprocessDllPath = Path::Combine(Path::GetDirectoryName(SelfHost::typeid->Assembly->Location), "CefSharp.BrowserSubprocess.Core.dll");
#ifdef NETCOREAPP
            auto browserSubprocessDll = System::Runtime::Loader::AssemblyLoadContext::Default->LoadFromAssemblyPath(browserSubprocessDllPath);
#else
            auto browserSubprocessDll = System::Reflection::Assembly::LoadFrom(browserSubprocessDllPath);
#endif
            auto browserSubprocessExecutableType = browserSubprocessDll->GetType("CefSharp.BrowserSubprocess.BrowserSubprocessExecutable");
            auto browserSubprocessExecutable = Activator::CreateInstance(browserSubprocessExecutableType);

            auto mainMethod = browserSubprocessExecutableType->GetMethod("MainSelfHost", System::Reflection::BindingFlags::Static | System::Reflection::BindingFlags::Public);
            auto argCount = mainMethod->GetParameters();

            auto methodArgs = gcnew array<Object^>(1);
            methodArgs[0] = args;

            auto exitCode = mainMethod->Invoke(nullptr, methodArgs);

            return (int)exitCode;
        }

    }
}

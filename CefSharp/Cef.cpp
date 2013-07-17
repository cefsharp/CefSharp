// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Cef.h"
#include "Settings.h"
#include "Internals\ClientApp.h"

using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

namespace CefSharp
{
    void Cef::Initialize(Settings^ settings)
    {
        // TODO: Remove if we conclude that we won't need it. The problem with it that we don't always have an EntryAssembly,
        // e.g. when running unit tests.
        //auto module = Assembly::GetEntryAssembly()->GetModules()[0];
        //auto hInstance = (HINSTANCE) Marshal::GetHINSTANCE(module).ToPointer();

        CefMainArgs main_args; //(hInstance);
        CefRefPtr<ClientApp> app(new ClientApp);

        int exitCode = CefExecuteProcess(main_args, app.get());
        
        if (exitCode >= 0)
        {
            throw gcnew Exception(
                "Failed to execute CEF process. Possible causes could be version mismatches (replacing libcef.dll etc. without " +
                "recompiling CefSharp). Error code was: " + exitCode);
        }
        
        CefInitialize(main_args, *(settings->_cefSettings), app.get());
    }

    void Cef::NavigateTo(Uri^ uri)
    {
        
    }
}
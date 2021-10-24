// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"

#include "SubProcessApp.h"
#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "Cef.h"

using namespace System::Collections::Generic;
using namespace System::Linq;
using namespace CefSharp::Internals;
using namespace CefSharp::RenderProcess;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        // Wrap CefAppUnmangedWrapper in a nice managed wrapper
        public ref class SubProcess
        {
        private:
            MCefRefPtr<CefAppUnmanagedWrapper> _cefApp;

        public:
            SubProcess(IRenderProcessHandler^ handler, IEnumerable<String^>^ args)
            {
                auto onBrowserCreated = gcnew Action<CefBrowserWrapper^>(this, &SubProcess::OnBrowserCreated);
                auto onBrowserDestroyed = gcnew Action<CefBrowserWrapper^>(this, &SubProcess::OnBrowserDestroyed);
                auto schemes = CefCustomScheme::ParseCommandLineArguments(args);
                auto enableFocusedNodeChanged = CommandLineArgsParser::HasArgument(args, CefSharpArguments::FocusedNodeChangedEnabledArgument);

                _cefApp = new CefAppUnmanagedWrapper(handler, schemes, enableFocusedNodeChanged, onBrowserCreated, onBrowserDestroyed);
            }

            !SubProcess()
            {
                _cefApp = nullptr;
            }

            ~SubProcess()
            {
                this->!SubProcess();
            }

            int Run()
            {
                auto hInstance = Process::GetCurrentProcess()->Handle;

                CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

                return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), nullptr);
            }

            virtual void OnBrowserCreated(CefBrowserWrapper^ cefBrowserWrapper)
            {

            }

            virtual void OnBrowserDestroyed(CefBrowserWrapper^ cefBrowserWrapper)
            {

            }

            static void EnableHighDPISupport()
            {
                CefEnableHighDPISupport();
            }

            static int ExecuteProcess(IEnumerable<String^>^ args)
            {
                auto hInstance = Process::GetCurrentProcess()->Handle;

                CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

                auto schemes = CefCustomScheme::ParseCommandLineArguments(args);

                CefRefPtr<CefApp> app = new SubProcessApp(schemes);

                return CefExecuteProcess(cefMainArgs, app, nullptr);
            }
        };
    }
}

// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "SubProcess.h"
#include "WcfEnabledSubProcess.h"
#include "BrowserSubprocessExecutable.h"

using namespace System;
using namespace CefSharp::Internals;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        /// <summary>
        /// WcfBrowserSubprocessExecutable provides the fundimental browser process handling for
        /// CefSharp.BrowserSubprocess.exe and can be used to self host the BrowserSubProcess in your
        /// existing application (preferred approach for .Net Core).
        /// If the <see cref="CefSharpArguments::WcfEnabledArgument"/> command line argument is
        /// present then the WcfEnabledSubProcess implementation is used.
        /// </summary>
        public ref class WcfBrowserSubprocessExecutable : BrowserSubprocessExecutable
        {
        public:
            WcfBrowserSubprocessExecutable()
            {

            }

            /// <summary>
            /// This function should be called from the application entry point function (typically Program.Main)
            /// to execute a secondary process e.g. gpu, renderer, utility
            /// This overload is specifically used for .Net 4.x. For hosting your own BrowserSubProcess
            /// it's preferable to use the Main method provided by this class.
            /// </summary>
            /// <returns>
            /// If called for the browser process (identified by no "type" command-line value) it will return immediately
            /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
            /// and then return the process exit code.
            /// </returns
            static int MainSelfHost(array<String^>^ args)
            {
                auto subProcess = gcnew WcfBrowserSubprocessExecutable();
                return subProcess->Main(args, nullptr);
            }

        protected:
            SubProcess^ GetSubprocess(IEnumerable<String^>^ args, int parentProcessId, IRenderProcessHandler^ handler) override
            {
                auto wcfEnabled = CommandLineArgsParser::HasArgument(args, CefSharpArguments::WcfEnabledArgument);
                if (wcfEnabled)
                {
                    return gcnew WcfEnabledSubProcess(parentProcessId, handler, args);
                }
                return gcnew SubProcess(handler, args);
            }
        };
    }
}

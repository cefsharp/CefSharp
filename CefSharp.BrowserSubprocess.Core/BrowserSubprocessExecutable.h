// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "SubProcess.h"

using namespace System;
using namespace CefSharp::Internals;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        /// <summary>
        /// BrowserSubprocessExecutable provides the fundimental browser process handling for
        /// CefSharp.BrowserSubprocess.exe and can be used to self host the BrowserSubProcess in your
        /// existing application (preferred approach for .Net Core).
        /// </summary>
        public ref class BrowserSubprocessExecutable
        {
        public:
            BrowserSubprocessExecutable()
            {

            }

            /// <summary>
            /// This function should be called from the application entry point function (typically Program.Main)
            /// to execute a secondary process e.g. gpu, plugin, renderer, utility
            /// This overload is specifically used for .Net Core. For hosting your own BrowserSubProcess
            /// it's preferable to use the Main method provided by this class.
            /// - Obtains the command line args via a call to Environment::GetCommandLineArgs
            /// - Calls CefEnableHighDPISupport before any other processing
            /// </summary>
            /// <returns>
            /// If called for the browser process (identified by no "type" command-line value) it will return immediately
            /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
            /// and then return the process exit code.
            /// </returns
            static int MainNetCore(IntPtr arg, int argLength)
            {
                SubProcess::EnableHighDPISupport();

                auto args = Environment::GetCommandLineArgs();

                auto subProcess = gcnew BrowserSubprocessExecutable();
                return subProcess->Main(args, nullptr);
            }

            /// <summary>
            /// This function should be called from the application entry point function (typically Program.Main)
            /// to execute a secondary process e.g. gpu, plugin, renderer, utility
            /// It can be used to run secondary processes (BrowserSubProcess) from your main applications executable
            /// or from a separate executable specified by the CefSettings.BrowserSubprocessPath value.
            /// CefSharp defaults to using the latter approach, a default implementation (CefSharp.BrowserSubProcess.exe) is
            /// supplied, see https://github.com/cefsharp/CefSharp/wiki/General-Usage#processes for more details.
            /// </summary>
            /// <param name="args">command line args</param>
            /// <returns>
            /// If called for the browser process (identified by no "type" command-line value) it will return immediately
            /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
            /// and then return the process exit code.
            /// </returns
            int Main(IEnumerable<String^>^ args)
            {
                return Main(args, nullptr);
            }

            /// <summary>
            /// This function should be called from the application entry point function (typically Program.Main)
            /// to execute a secondary process e.g. gpu, plugin, renderer, utility
            /// It can be used to run secondary processes (BrowserSubProcess) from your main applications executable
            /// or from a separate executable specified by the CefSettings.BrowserSubprocessPath value.
            /// CefSharp defaults to using the latter approach, a default implementation (CefSharp.BrowserSubProcess.exe) is
            /// supplied, see https://github.com/cefsharp/CefSharp/wiki/General-Usage#processes for more details.
            /// </summary>
            /// <param name="args">command line args</param>
            /// <param name="handler">An option IRenderProcessHandler implementation, use null if no handler is required</param>
            /// <returns>
            /// If called for the browser process (identified by no "type" command-line value) it will return immediately
            /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
            /// and then return the process exit code.
            /// </returns>
            int Main(IEnumerable<String^>^ args, IRenderProcessHandler^ handler)
            {
                auto type = CommandLineArgsParser::GetArgumentValue(args, CefSharpArguments::SubProcessTypeArgument);

                if (String::IsNullOrEmpty(type))
                {
                    //If --type param missing from command line CEF/Chromium assums
                    //this is the main process (as all subprocesses must have a type param).
                    //Return -1 to indicate this behaviour.
                    return -1;
                }

                auto parentProcessId = -1;

                // The Crashpad Handler doesn't have any HostProcessIdArgument, so we must not try to
                // parse it lest we want an ArgumentNullException.
                if (type != "crashpad-handler")
                {
                    parentProcessId = int::Parse(CommandLineArgsParser::GetArgumentValue(args, CefSharpArguments::HostProcessIdArgument));
                    if (CommandLineArgsParser::HasArgument(args, CefSharpArguments::ExitIfParentProcessClosed))
                    {
                        ParentProcessMonitor::StartMonitorTask(parentProcessId);
                    }
                }

                // Use our custom subProcess provides features like EvaluateJavascript
                if (type == "renderer")
                {
                    auto subProcess = GetSubprocess(args, parentProcessId, handler);

                    try
                    {
                        return subProcess->Run();
                    }
                    finally
                    {
                        delete subProcess;
                    }
                }

                return SubProcess::ExecuteProcess(args);
            }

        protected:
            virtual SubProcess^ GetSubprocess(IEnumerable<String^>^ args, int parentProcessId, IRenderProcessHandler^ handler)
            {
                return gcnew SubProcess(handler, args);
            }
        };
    }
}

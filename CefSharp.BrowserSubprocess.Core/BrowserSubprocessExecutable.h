// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "SubProcess.h"
#include "WcfEnabledSubProcess.h"

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

            int Main(IEnumerable<String^>^ args, IRenderProcessHandler^ handler)
            {
                int result;
                auto type = CommandLineArgsParser::GetArgumentValue(args, CefSharpArguments::SubProcessTypeArgument);

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
                        result = subProcess->Run();
                    }
                    finally
                    {
                        delete subProcess;
                    }
                }
                else
                {
                    result = SubProcess::ExecuteProcess(args);
                }

                return result;
            }

        protected:
            virtual SubProcess^ GetSubprocess(IEnumerable<String^>^ args, int parentProcessId, IRenderProcessHandler^ handler)
            {
                return gcnew SubProcess(handler, args);
            }
        };
    }
}

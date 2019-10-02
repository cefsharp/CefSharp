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
        public ref class WcfBrowserSubprocessExecutable : BrowserSubprocessExecutable
        {
        public:
            WcfBrowserSubprocessExecutable()
            {

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

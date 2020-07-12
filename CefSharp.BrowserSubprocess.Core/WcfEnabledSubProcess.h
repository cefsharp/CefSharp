// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "SubProcess.h"
#include "CefBrowserWrapper.h"

using namespace System;
using namespace CefSharp::RenderProcess;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        public ref class WcfEnabledSubProcess : SubProcess
        {
        private:
            Nullable<int> _parentBrowserId;

            /// <summary>
            /// The PID for the parent (browser) process
            /// </summary>
            int _parentProcessId;

        public:
            WcfEnabledSubProcess(int parentProcessId, IRenderProcessHandler^ handler, IEnumerable<String^>^ args) : SubProcess(handler,args)
            {
                _parentProcessId = parentProcessId;
            }

            void OnBrowserCreated(CefBrowserWrapper^ browser) override;
            void OnBrowserDestroyed(CefBrowserWrapper^ browser) override;
        };
    }
}

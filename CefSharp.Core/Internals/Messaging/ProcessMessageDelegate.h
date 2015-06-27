// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <set>

#include "include\cef_base.h"
#include "include\cef_app.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            //Specific process message handler classes can inherit from this one
            class ProcessMessageDelegate : public virtual CefBase
            {
            public:
                //Will be called for uhandled received process messages.
                //Returns false if message wasn't handled, true if it was
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) = 0;

                IMPLEMENT_REFCOUNTING(ProcessMessageDelegate);
            };

            typedef std::set<CefRefPtr<ProcessMessageDelegate>> ProcessMessageDelegateSet;
        }
    }
}
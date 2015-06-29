// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once
    
#include "../CefSharp.Core/Internals/Messaging/ProcessMessageDelegate.h"
    
namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    namespace Internals
    {
        namespace Messaging
        {
            //This class handles incoming evaluate script messages and responses to them after fulfillment.
            class EvaluateScriptDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(EvaluateScriptDelegate);

                CefRefPtr<CefAppUnmanagedWrapper> _appUnmanagedWrapper;
            public:
                EvaluateScriptDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper);
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
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

                CefRefPtr<CefProcessMessage> EvaluateScript(int browserId, int frameId, int64 callbackId, CefString script);
                CefRefPtr<CefProcessMessage> EvaluateScriptInFrame(CefRefPtr<CefFrame> frame, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry);
                CefRefPtr<CefProcessMessage> EvaluateScriptInContext(CefRefPtr<CefV8Context> context, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry);
            public:
                EvaluateScriptDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper);
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
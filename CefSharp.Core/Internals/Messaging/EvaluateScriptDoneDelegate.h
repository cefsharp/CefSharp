// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once
    
#include "ProcessMessageDelegate.h"
    
namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            //This class will handle result messages of eval script calls
            class EvaluateScriptDoneDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(EvaluateScriptDoneDelegate);

                gcroot<Dictionary<int, IJavascriptCallbackFactory^>^> _callbackFactories;
                gcroot<PendingTaskRepository<JavascriptResponse^>^> _pendingTasks;
            public:
                EvaluateScriptDoneDelegate(PendingTaskRepository<JavascriptResponse^>^ pendingTasks, Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactories);
                Task<JavascriptResponse^>^ EvaluateScriptAsync(CefRefPtr<CefBrowser> cefBrowser, int browserId, int frameId, String^ script, Nullable<TimeSpan> timeout);
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
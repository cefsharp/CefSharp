#pragma once

#include "TaskDoneDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            class EvaluateScriptDoneDelegate : public TaskDoneDelegate
            {
            protected:
                virtual bool CanHandle(CefRefPtr<CefProcessMessage> message) override;
            public:
				EvaluateScriptDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks, Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory);

                Task<JavascriptResponse^>^ EvaluateScriptAsync(CefRefPtr<CefBrowser> cefBrowser, int frameId, String^ script, Nullable<TimeSpan> timeout);
            };
        }
    }
}
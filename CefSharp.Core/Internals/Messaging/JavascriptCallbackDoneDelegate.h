#pragma once

#include "TaskDoneDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            class JavascriptCallbackDoneDelegate : public TaskDoneDelegate
            {
            protected:
                virtual bool CanHandle(CefRefPtr<CefProcessMessage> message) override;
            public:
				JavascriptCallbackDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks, Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory);
            };
        }
    }
}

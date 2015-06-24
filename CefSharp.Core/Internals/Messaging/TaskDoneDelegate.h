#pragma once

#include "../JavascriptCallbackImplFactory.h"
#include "ProcessMessageDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            class TaskDoneDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(TaskDoneDelegate);

				gcroot<Dictionary<int, IJavascriptCallbackFactory^>^> _callbackFactory;

				void FinishTask(int browserId, int64 callbackId, bool success, CefRefPtr<CefListValue> message, IJavascriptCallbackFactory^ callbackFactory);
				JavascriptResponse^ CreateResponse(bool success, CefRefPtr<CefListValue> message, IJavascriptCallbackFactory^ callbackFactory);
            protected:
                gcroot<Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^> _pendingTasks;

                virtual bool CanHandle(CefRefPtr<CefProcessMessage> message) = 0;
            public:
				TaskDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks, Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory);

                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
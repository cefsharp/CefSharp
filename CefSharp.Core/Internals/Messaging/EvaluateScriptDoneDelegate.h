#pragma once

#include "ProcessMessageDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            class EvaluateScriptDoneDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(EvaluateScriptDoneDelegate);

                gcroot<PendingTaskRepository<JavascriptResponse^>^> _pendingTasks;

                void FinishTask(int64 callbackId, bool success, CefRefPtr<CefListValue> message);

                JavascriptResponse^ CreateResponse(bool success, CefRefPtr<CefListValue> message);
            public:
                EvaluateScriptDoneDelegate(PendingTaskRepository<JavascriptResponse^>^ pendingTasks);

                Task<JavascriptResponse^>^ EvaluateScriptAsync(CefRefPtr<CefBrowser> cefBrowser, int browserId, int frameId, String^ script, Nullable<TimeSpan> timeout);

                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
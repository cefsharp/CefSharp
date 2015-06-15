#include "Stdafx.h"
#include "Messages.h"
#include "../Serialization/Primitives.h"
#include "../Serialization/V8Serialization.h"
#include "EvaluateScriptDoneDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Serialization;

        namespace Messaging
        {
			EvaluateScriptDoneDelegate::EvaluateScriptDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks,
				Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory)
				:TaskDoneDelegate(pendingTasks, callbackFactory)
            {

            }

            bool EvaluateScriptDoneDelegate::CanHandle(CefRefPtr<CefProcessMessage> message)
            {
                auto name = message->GetName();
                return name == kEvaluateJavascriptDone;
            }

            Task<JavascriptResponse^>^ EvaluateScriptDoneDelegate::EvaluateScriptAsync(CefRefPtr<CefBrowser> cefBrowser, int frameId, String^ script, Nullable<TimeSpan> timeout)
            {
                PendingTaskRepository<JavascriptResponse^>^ pendingTasks = nullptr;
                TaskCompletionSource<JavascriptResponse^>^ completionSource = nullptr;
                auto browserId = cefBrowser->GetIdentifier();

                if (_pendingTasks->TryGetValue(browserId, pendingTasks))
                {
                    auto callbackId = timeout.HasValue ? pendingTasks->CreatePendingTaskWithTimeout(completionSource, timeout.Value) :
                        pendingTasks->CreatePendingTask(completionSource);

                    auto message = CefProcessMessage::Create(kEvaluateJavascript);
                    auto argList = message->GetArgumentList();
                    argList->SetInt(0, browserId);
                    argList->SetInt(1, frameId);
                    SetInt64(callbackId, argList, 2);
                    argList->SetString(3, StringUtils::ToNative(script));

                    cefBrowser->SendProcessMessage(CefProcessId::PID_RENDERER, message);

                    return completionSource->Task;
                }
                else
                {
                    throw gcnew InvalidOperationException("");
                }
            }         
        }
    }
}
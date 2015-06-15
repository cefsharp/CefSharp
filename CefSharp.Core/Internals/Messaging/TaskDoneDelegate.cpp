#include "Stdafx.h"
#include "../Serialization/Primitives.h"
#include "../Serialization/V8Serialization.h"
#include "TaskDoneDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Serialization;

        namespace Messaging
        {
			TaskDoneDelegate::TaskDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks,
				Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory)
				:_pendingTasks(pendingTasks), _callbackFactory(callbackFactory)
            {

            }

            bool TaskDoneDelegate::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
            {
                auto handled = false;
				IJavascriptCallbackFactory^ callbackFactory;
				if (CanHandle(message) && _callbackFactory->TryGetValue(browser->GetIdentifier(), callbackFactory))
                {
                    auto argList = message->GetArgumentList();
                    auto success = argList->GetBool(0);
                    auto callbackId = GetInt64(argList, 1);

					FinishTask(browser->GetIdentifier(), callbackId, success, argList, callbackFactory);

                    handled = true;
                }

                return handled;
            }

			void TaskDoneDelegate::FinishTask(int browserId, int64 callbackId, bool success, CefRefPtr<CefListValue> message, IJavascriptCallbackFactory^ callbackFactory)
            {
                PendingTaskRepository<JavascriptResponse^>^ pendingTasks = nullptr;
                if (_pendingTasks->TryGetValue(browserId, pendingTasks))
                {
                    auto pendingTask = pendingTasks->RemovePendingTask(callbackId);
                    if (pendingTask != nullptr)
                    {
                        pendingTask->SetResult(CreateResponse(success, message, callbackFactory));
                    }
                }
            }

			JavascriptResponse^ TaskDoneDelegate::CreateResponse(bool success, CefRefPtr<CefListValue> message, IJavascriptCallbackFactory^ callbackFactory)
            {
                auto result = gcnew JavascriptResponse();
                result->Success = success;

                if (success)
                {
                    result->Result = DeserializeV8Object(message, 2, callbackFactory);
                }
                else
                {
                    result->Message = StringUtils::ToClr(message->GetString(2));
                }

                return result;
            }
        }
    }
}
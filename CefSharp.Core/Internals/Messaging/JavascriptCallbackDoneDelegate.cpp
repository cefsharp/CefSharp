#include "Stdafx.h"
#include "Messages.h"
#include "JavascriptCallbackDoneDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
			JavascriptCallbackDoneDelegate::JavascriptCallbackDoneDelegate(Dictionary<int, PendingTaskRepository<JavascriptResponse^>^>^ pendingTasks, 
				Dictionary<int, IJavascriptCallbackFactory^>^ callbackFactory)
                :TaskDoneDelegate(pendingTasks, callbackFactory)
            {
            }

            bool JavascriptCallbackDoneDelegate::CanHandle(CefRefPtr<CefProcessMessage> message)
            {
                auto name = message->GetName();
                return name == kJavascriptCallbackDone;
            }
        }
    }
}


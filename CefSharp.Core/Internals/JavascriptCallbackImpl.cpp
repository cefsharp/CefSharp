#include "Stdafx.h"
#include "JavascriptCallbackImpl.h"
#include "Messaging/Messages.h"
#include "Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Messaging;
        using namespace Serialization;

        JavascriptCallbackImpl::JavascriptCallbackImpl(JavascriptCallback^ callback, PendingTaskRepository<JavascriptResponse^>^ pendingTasks, WeakReference^ browserWrapper)
            :_callback(callback), _pendingTasks(pendingTasks)
        {
            _browserWrapper = browserWrapper;
        }

        Task<JavascriptResponse^>^ JavascriptCallbackImpl::ExecuteAsync(array<Object^>^ parameters)
        {
            auto browser = GetBrowser();
            if (browser.get())
            {
                TaskCompletionSource<JavascriptResponse^>^ taskCompletion;
                auto doneCallbackId = _pendingTasks->CreatePendingTask(taskCompletion);
                auto callbackMessage = CreateCallMessage(doneCallbackId, parameters);
                browser->SendProcessMessage(CefProcessId::PID_RENDERER, callbackMessage);

                return taskCompletion->Task;
            }
            else
            {
                throw gcnew InvalidOperationException("");
            }
        }

        CefRefPtr<CefProcessMessage> JavascriptCallbackImpl::CreateCallMessage(int64 doneCallbackId, array<Object^>^ parameters)
        {
            auto result = CefProcessMessage::Create(kJavascriptCallback);
            auto argList = result->GetArgumentList();
            SetInt64(_callback->Id, argList, 0);
            SetInt64(doneCallbackId, argList, 1);
            auto paramList = CefListValue::Create();
            for (auto i = 0; i < parameters->Length; i++)
            {
                SerializeV8Object(parameters[i], paramList, i);
            }
            argList->SetList(2, paramList);
            return result;
        }

        CefRefPtr<CefProcessMessage> JavascriptCallbackImpl::CreateDestroyMessage()
        {
            auto result = CefProcessMessage::Create(kJavascriptCallbackDestroy);
            auto argList = result->GetArgumentList();
			SetInt64(_callback->Id, argList, 0);
            return result;
        }

        CefRefPtr<CefBrowser> JavascriptCallbackImpl::GetBrowser()
        {
            CefRefPtr<CefBrowser> result;
            if (_browserWrapper->IsAlive)
            {
                auto wrapper = static_cast<CefSharpBrowserWrapper^>(_browserWrapper->Target);
                result = wrapper == nullptr ? nullptr : wrapper->Browser.get();
            }
            return result;
        }

        void JavascriptCallbackImpl::DisposedGuard()
        {
            if (_disposed)
            {
                throw gcnew ObjectDisposedException("JavascriptCallbackImpl");
            }
        }

        JavascriptCallbackImpl::!JavascriptCallbackImpl()
        {
            auto browser = GetBrowser();
            if (browser.get())
            {
                browser->SendProcessMessage(CefProcessId::PID_RENDERER, CreateDestroyMessage());
            }
            _disposed = true;
        }
    }
}

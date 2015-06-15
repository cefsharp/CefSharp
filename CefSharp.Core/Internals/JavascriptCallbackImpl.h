#pragma once

#include "include/cef_base.h"
#include "include/cef_app.h"

#include "CefSharpBrowserWrapper.h"

using namespace System;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    namespace Internals
    {
        ref class JavascriptCallbackImpl : public IJavascriptCallback
        {
        private:
            WeakReference^ _browserWrapper;
            JavascriptCallback^ _callback;
            Messaging::PendingTaskRepository<JavascriptResponse^>^ _pendingTasks;
            bool _disposed;

            CefRefPtr<CefProcessMessage> CreateCallMessage(int64 doneCallbackId, array<Object^>^ parameters);
            CefRefPtr<CefProcessMessage> CreateDestroyMessage();
            CefRefPtr<CefBrowser> GetBrowser();
            void DisposedGuard();
        public:
            JavascriptCallbackImpl(JavascriptCallback^ callback, Messaging::PendingTaskRepository<JavascriptResponse^>^ pendingTasks, WeakReference^ browser);

            virtual Task<JavascriptResponse^>^ ExecuteAsync(array<Object^>^ parameters) override;

            ~JavascriptCallbackImpl() { this->!JavascriptCallbackImpl(); }
            !JavascriptCallbackImpl();
        };
    }
}
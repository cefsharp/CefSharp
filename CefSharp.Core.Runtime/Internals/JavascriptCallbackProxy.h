// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_app.h"

#include "..\ManagedCefBrowserAdapter.h"
#include "Internals\CefBrowserWrapper.h"

using namespace System::Threading::Tasks;
using namespace CefSharp::JavascriptBinding;

namespace CefSharp
{
    namespace Internals
    {
        private ref class JavascriptCallbackProxy : public IJavascriptCallback
        {
        private:
            WeakReference<IBrowserAdapter^>^ _browserAdapter;
            JavascriptCallback^ _callback;
            PendingTaskRepository<JavascriptResponse^>^ _pendingTasks;
            bool _disposed;

            CefRefPtr<CefProcessMessage> CreateDestroyMessage();
            IBrowser^ GetBrowser();
            IJavascriptNameConverter^ GetJavascriptNameConverter();
            void DisposedGuard();
        public:
            JavascriptCallbackProxy(JavascriptCallback^ callback, PendingTaskRepository<JavascriptResponse^>^ pendingTasks, WeakReference<IBrowserAdapter^>^ browserAdapter)
                :_callback(callback), _pendingTasks(pendingTasks), _disposed(false)
            {
                _browserAdapter = browserAdapter;
            }

            ~JavascriptCallbackProxy()
            {
                this->!JavascriptCallbackProxy();
            }

            !JavascriptCallbackProxy()
            {
                auto browser = GetBrowser();
                if (browser != nullptr && !browser->IsDisposed)
                {
                    auto browserWrapper = static_cast<CefBrowserWrapper^>(browser)->Browser;
                    auto frame = browserWrapper->GetFrameByIdentifier(StringUtils::ToNative(_callback->FrameId));
                    if (frame.get() && frame->IsValid())
                    {
                        frame->SendProcessMessage(CefProcessId::PID_RENDERER, CreateDestroyMessage());
                    }
                }
                _disposed = true;
            }

            virtual Task<JavascriptResponse^>^ ExecuteAsync(cli::array<Object^>^ parameters);
            virtual Task<JavascriptResponse^>^ ExecuteWithTimeoutAsync(Nullable<TimeSpan> timeout, cli::array<Object^>^ parameters);

            virtual property Int64 Id
            {
                Int64 get();
            }

            virtual property bool IsDisposed
            {
                bool get();
            }

            virtual property bool CanExecute
            {
                bool get();
            }
        };
    }
}

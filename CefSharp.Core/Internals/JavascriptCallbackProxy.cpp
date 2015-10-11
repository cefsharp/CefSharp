// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "JavascriptCallbackProxy.h"
#include "Messaging/Messages.h"
#include "Serialization/Primitives.h"
#include "Serialization/V8Serialization.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        Task<JavascriptResponse^>^ JavascriptCallbackProxy::ExecuteAsync(cli::array<Object^>^ parameters)
        {
            DisposedGuard();

            auto browser = GetBrowser();
            if (browser == nullptr)
            {
                throw gcnew InvalidOperationException("Browser instance is null.");
            }

            auto browserWrapper = static_cast<CefSharpBrowserWrapper^>(browser);

            auto doneCallback = _pendingTasks->CreatePendingTask(Nullable<TimeSpan>());

            auto callbackMessage = CefProcessMessage::Create(kJavascriptCallbackRequest);
            auto argList = callbackMessage->GetArgumentList();
            SetInt64(_callback->FrameId, argList, 0);
            SetInt64(doneCallback.Key, argList, 1);
            SetInt64(_callback->Id, argList, 2);
            auto paramList = CefListValue::Create();
            for (int i = 0; i < parameters->Length; i++)
            {
                auto param = parameters[i];
                SerializeV8Object(param, paramList, i);
            }
            argList->SetList(3, paramList);

            browserWrapper->SendProcessMessage(CefProcessId::PID_RENDERER, callbackMessage);

            return doneCallback.Value->Task;
        }

        CefRefPtr<CefProcessMessage> JavascriptCallbackProxy::CreateDestroyMessage()
        {
            auto result = CefProcessMessage::Create(kJavascriptCallbackDestroyRequest);
            auto argList = result->GetArgumentList();
            SetInt64(_callback->Id, argList, 0);
            SetInt64(_callback->FrameId, argList, 1);
            return result;
        }

        IBrowser^ JavascriptCallbackProxy::GetBrowser()
        {
            IBrowser^ result = nullptr;
            if (_browserAdapter->IsAlive)
            {
                auto browserAdapter = static_cast<ManagedCefBrowserAdapter^>(_browserAdapter->Target);
                result = browserAdapter->GetBrowser(_callback->BrowserId);
            }
            return result;
        }

        bool JavascriptCallbackProxy::IsDisposed::get()
        {
            return _disposed;
        }

        void JavascriptCallbackProxy::DisposedGuard()
        {
            if (_disposed)
            {
                throw gcnew ObjectDisposedException("JavascriptCallbackProxy");
            }
        }
    }
}
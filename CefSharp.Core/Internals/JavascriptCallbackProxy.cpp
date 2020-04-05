// Copyright © 2015 The CefSharp Authors. All rights reserved.
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
            return ExecuteWithTimeoutAsync(Nullable<TimeSpan>(), parameters);
        }

        Task<JavascriptResponse^>^ JavascriptCallbackProxy::ExecuteWithTimeoutAsync(Nullable<TimeSpan> timeout, cli::array<Object^>^ parameters)
        {
            DisposedGuard();

            auto browser = GetBrowser();
            if (browser == nullptr)
            {
                throw gcnew InvalidOperationException("Browser instance is null. Check CanExecute before calling this method.");
            }

            auto browserWrapper = static_cast<CefBrowserWrapper^>(browser);

            auto doneCallback = _pendingTasks->CreatePendingTask(timeout);

            auto callbackMessage = CefProcessMessage::Create(kJavascriptCallbackRequest);
            auto argList = callbackMessage->GetArgumentList();
            SetInt64(argList, 0, doneCallback.Key);
            SetInt64(argList, 1, _callback->Id);
            auto paramList = CefListValue::Create();
            for (int i = 0; i < parameters->Length; i++)
            {
                auto param = parameters[i];
                SerializeV8Object(paramList, i, param);
            }
            argList->SetList(2, paramList);

            auto frame = browserWrapper->Browser->GetFrame(_callback->FrameId);

            if (frame.get() && frame->IsValid())
            {
                frame->SendProcessMessage(CefProcessId::PID_RENDERER, callbackMessage);

                return doneCallback.Value->Task;
            }
            else
            {
                auto invalidFrameResponse = gcnew JavascriptResponse();
                invalidFrameResponse->Success = false;
                invalidFrameResponse->Message = "Frame with Id:" + _callback->FrameId + " is no longer valid.";

                Task::FromResult(invalidFrameResponse);
            }
        }

        CefRefPtr<CefProcessMessage> JavascriptCallbackProxy::CreateDestroyMessage()
        {
            auto result = CefProcessMessage::Create(kJavascriptCallbackDestroyRequest);
            auto argList = result->GetArgumentList();
            SetInt64(argList, 0, _callback->Id);
            return result;
        }

        IBrowser^ JavascriptCallbackProxy::GetBrowser()
        {
            IBrowser^ result = nullptr;
            if (_browserAdapter->IsAlive)
            {
                auto browserAdapter = static_cast<IBrowserAdapter^>(_browserAdapter->Target);
                if (!browserAdapter->IsDisposed)
                {
                    result = browserAdapter->GetBrowser(_callback->BrowserId);
                }
            }
            return result;
        }

        Int64 JavascriptCallbackProxy::Id::get()
        {
            DisposedGuard();

            return _callback->Id;
        }

        bool JavascriptCallbackProxy::IsDisposed::get()
        {
            return _disposed;
        }

        bool JavascriptCallbackProxy::CanExecute::get()
        {
            if (_disposed)
            {
                return false;
            }

            auto browser = GetBrowser();
            if (browser == nullptr)
            {
                return false;
            }

            //If the frame Id is still valid then we can attemp to execute the callback
            auto frame = browser->GetFrame(_callback->FrameId);
            if (frame == nullptr)
            {
                return false;
            }

            return frame->IsValid;
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

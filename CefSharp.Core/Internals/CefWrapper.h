// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Diagnostics;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefWrapper abstract
        {
        protected:
            bool _disposed;

            void ThrowIfDisposed()
            {
                if (_disposed)
                {
                    auto type = GetType();

                    throw gcnew ObjectDisposedException(gcnew String(L"This instance of " + type->GetInterfaces()[0]->FullName + " been disposed!"));
                }
            }

            void ThrowIfExecutedOnNonCefUiThread()
            {
                if (!CefCurrentlyOn(CefThreadId::TID_UI))
                {
                    auto st = gcnew StackTrace(gcnew StackFrame(1));
                    auto method = st->GetFrame(0)->GetMethod();

                    throw gcnew Exception(gcnew String(method->Name + L" must be called on the CEF UI Thread, by default this is different " +
                        "to your applications UI Thread. You can use Cef.UIThreadTaskFactory to execute code on the CEF UI Thread. " +
                        "IBrowserProcessHandler.OnContextInitialized/IRequestContextHandler.OnRequestContextInitialized/ILifeSpanHandler.OnAfterCreated are called directly on the CEF UI Thread."));
                }
            }

        internal:
            CefWrapper() : _disposed(false)
            {

            };

        public:
            virtual property bool IsDisposed
            {
                bool get()
                {
                    return _disposed;
                }
            }
        };
    }
}

// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/base/cef_logging.h>

using namespace System::Threading;

namespace CefSharp
{
    namespace Internals
    {

        private class ReportUnhandledExceptions
        {
        public:
            static void Report(String^ str, Exception^ exception)
            {
                auto msg = String::Format(gcnew String(L"{0}: {1}"), str, exception->ToString());
                LOG(ERROR) << StringUtils::ToNative(str).ToString();
                // Throw the exception from a threadpool thread so that
                // AppDomain::UnhandledException event handler will get called 
                // with the exception.
                ThreadPool::UnsafeQueueUserWorkItem(
                    gcnew WaitCallback(&ReportUnhandledExceptions::ThrowUnhandledException),
                    gcnew Tuple<String^, Exception^>(str, exception));
            }
        private:
            // Throw the exception, this method executes on the thread pool
            // in order to report a .net exception from a CEF thread.
            static void ThrowUnhandledException(Object ^state)
            {
                Tuple<String^, Exception^>^ params = dynamic_cast<Tuple<String^, Exception^>^>(state);
                throw gcnew InvalidOperationException(params->Item1, params->Item2);
            }
        };
    }
}



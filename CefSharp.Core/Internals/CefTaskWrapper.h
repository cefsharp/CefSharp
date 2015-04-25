// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/base/cef_logging.h>

using namespace System;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        private class CefTaskWrapper
        {
        private:
            // Throw the exception, this method executes on the thread pool
            // in order to report a .net exception from a CEF thread.
            static void ReportUnhandledException(Object ^state)
            {
                Exception ^e = dynamic_cast<Exception^>(state);
                throw gcnew InvalidOperationException(gcnew String(L"CefTaskWrapper caught an unexpected exception. This should never happen, please contact CefSharp for assistance."), e);
            }

        public:
            gcroot<Task^> _task;
            gcroot<ITaskScheduler^> _scheduler;

            CefTaskWrapper(Task^ task, ITaskScheduler^ scheduler) :
                _task(task),
                _scheduler(scheduler)
            {
            };

            ~CefTaskWrapper()
            {
                delete _task;
            }

            void Execute()
            {
                try
                {
                    _scheduler->ExecuteTask(_task);
                }
                catch (Exception^ e)
                {
                    // This should never ever happen.
                    // If this occurs then someone has broken a .Net ThreadScheduler/Task invariant
                    // i.e. trying to run a task on the wrong scheduler, or some
                    // weird exception during task completion. 
                    auto msg = String::Format(gcnew String(L"CefTaskWrapper caught an unexpected exception. This should never happen, please contact CefSharp for assistance: {0}"), e->ToString());
                    LOG(ERROR) << StringUtils::ToNative(msg).ToString();
                    // Throw the exception from a threadpool thread in hopes that
                    // AppDomain::UnhandledException event handler will get called 
                    // with the exception.
                    ThreadPool::UnsafeQueueUserWorkItem(
                        gcnew WaitCallback(&CefTaskWrapper::ReportUnhandledException),
                        static_cast<Object^>(e));
                }
            };

            IMPLEMENT_REFCOUNTING(CefTaskWrapper)
        };
    }
}
// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_task.h"

#include "ReportUnhandledExceptions.h"

using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Internals
    {
        private class CefTaskWrapper : public CefTask
        {
        private:
            gcroot<Task^> _task;
            gcroot<ITaskScheduler^> _scheduler;
        public:
            CefTaskWrapper(Task^ task, ITaskScheduler^ scheduler) :
                _task(task),
                _scheduler(scheduler)
            {
            };

            virtual void Execute() OVERRIDE
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
                    auto msg = gcnew String(L"CefTaskWrapper caught an unexpected exception. This should never happen, please contact CefSharp for assistance");
                    ReportUnhandledExceptions::Report(msg, e);
                }
            };

            IMPLEMENT_REFCOUNTING(CefTaskWrapper)
        };
    }
}
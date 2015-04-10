// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <include/cef_runnable.h>

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
                catch (Exception^)
                {

                }
            };

            IMPLEMENT_REFCOUNTING(CefTaskWrapper)
        };

        public ref class CefTaskScheduler : TaskScheduler, ITaskScheduler
        {
        public:
            cef_thread_id_t _thread;

            CefTaskScheduler(cef_thread_id_t thread) :
                _thread(thread)
            {
            };

            virtual void QueueTask(Task^ task) override
            {
                auto taskwrapper = CefRefPtr<CefTaskWrapper>(new CefTaskWrapper(task, this));

                CefPostTask(_thread, NewCefRunnableMethod(taskwrapper.get(), &CefTaskWrapper::Execute));
            };

            virtual void ExecuteTask(Task^ task)
            {
                TryExecuteTask(task);
            };

        protected:
            virtual bool TryExecuteTaskInline(Task^ task, bool taskWasPreviouslyQueued) override
            {
                return false;
            };

            virtual IEnumerable<Task^>^ GetScheduledTasks() override
            {
                return nullptr;
            };
        };
    }
}
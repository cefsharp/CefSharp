// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <include/cef_runnable.h>

using namespace System;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    ref class CefTaskScheduler;

    class CefTaskWrapper
    {
    public:
        gcroot<Task^> _task;
        gcroot<CefTaskScheduler^> _scheduler;
        void Execute();

        CefTaskWrapper(Task^ task, CefTaskScheduler^ scheduler) :
            _task(task),
            _scheduler(scheduler)
        {
        };

        ~CefTaskWrapper()
        {
            delete _task;
            
            //TODO: Currently can't delete _scheduler for some reason
            //delete _scheduler;
        }

        IMPLEMENT_REFCOUNTING(CefTaskWrapper)
    };

    public ref class CefTaskScheduler : TaskScheduler
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

        void ExecuteTask(CefRefPtr<CefTaskWrapper> wapper)
        {
            TryExecuteTask(wapper->_task);
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
        typedef void(*Executor)(CefTaskWrapper*);
    };
}
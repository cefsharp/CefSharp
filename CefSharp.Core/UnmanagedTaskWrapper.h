
#include "Stdafx.h"
#include <include/cef_runnable.h>
#pragma once


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
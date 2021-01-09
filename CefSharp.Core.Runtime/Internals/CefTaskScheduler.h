// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_task.h"
#include "CefTaskWrapper.h"

using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefTaskScheduler : TaskScheduler, ITaskScheduler
        {
        public:
            CefThreadId _thread;

            CefTaskScheduler(CefThreadId thread) :
                _thread(thread)
            {
            };

            virtual void QueueTask(Task^ task) override
            {
                CefRefPtr<CefTask> taskWrapper = new CefTaskWrapper(task, this);

                CefPostTask(_thread, taskWrapper);
            };

            virtual void ExecuteTask(Task^ task)
            {
                TryExecuteTask(task);
            };

            static bool CurrentlyOnThread(CefThreadId threadId)
            {
                return CefCurrentlyOn(threadId);
            }

            static void EnsureOn(CefThreadId threadId, String^ context)
            {
                if (!CefCurrentlyOn(threadId))
                {
                    throw gcnew InvalidOperationException(String::Format("Executed '{0}' on incorrect thread. This method expects to run on the CEF {1} thread!", context, ((CefThreadIds)threadId).ToString()));
                }
            }

            static void EnsureOn(CefThreadIds threadId, String^ context)
            {
                EnsureOn((CefThreadId)threadId, context);
            }

        protected:
            virtual bool TryExecuteTaskInline(Task^ task, bool taskWasPreviouslyQueued) override
            {
                // You might think this method might not get called,
                // but a .ContinueWith(..., TaskContinuationOpation.ExecuteSyncronously)
                // will probably end up calling this method,
                // so assume the callers know what they're doing and execute the task
                // inline on this thread (if the current thread is correct for this scheduler)
                if (CefCurrentlyOn(_thread))
                {
                    return TryExecuteTask(task);
                }
                return false;
            };

            virtual IEnumerable<Task^>^ GetScheduledTasks() override
            {
                return nullptr;
            };
        };
    }
}
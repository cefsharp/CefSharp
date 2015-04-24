// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

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
                    // This should never ever happen.
                    // If this occurs then someone has broken a .Net ThreadScheduler/Task invariant
                    // i.e. trying to run a task on the wrong scheduler, or some
                    // weird exception during task completion. 
                    // TODO: We need to forward this exception somewhere...
                }
            };

            IMPLEMENT_REFCOUNTING(CefTaskWrapper)
        };
    }
}
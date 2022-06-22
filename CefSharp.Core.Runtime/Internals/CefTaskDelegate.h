// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_task.h"

#include "ReportUnhandledExceptions.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefTaskDelegate : public CefTask
        {
        private:
            gcroot<Action^> _action;
        public:
            CefTaskDelegate(Action^ action) :
                _action(action)
            {
            };

            virtual void Execute() override
            {
                try
                {
                    _action->Invoke();
                }
                catch (Exception^ e)
                {
                    auto msg = gcnew String(L"CefTaskDelegate caught an unexpected exception. This exception has been redirected onto the ThreadPool, add a try catch.");
                    ReportUnhandledExceptions::Report(msg, e);
                }
            };

            IMPLEMENT_REFCOUNTINGM(CefTaskDelegate);
        };
    }
}

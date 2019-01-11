// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_callback.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefCompletionCallbackAdapter : public CefCompletionCallback
        {
        private:
            gcroot<ICompletionCallback^> _handler;

        public:
            CefCompletionCallbackAdapter(ICompletionCallback^ handler)
            {
                _handler = handler;
            }

            ~CefCompletionCallbackAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            void OnComplete() OVERRIDE
            {
                _handler->OnComplete();
            }

            IMPLEMENT_REFCOUNTING(CefCompletionCallbackAdapter);
        };
    }
}

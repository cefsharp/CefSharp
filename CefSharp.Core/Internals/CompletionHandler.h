// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_cookie.h"

namespace CefSharp
{
    private class CompletionHandler : public CefCompletionCallback
    {
    private:
        gcroot<ICompletionHandler^> _handler;

    public:
        CompletionHandler(ICompletionHandler^ handler)
        {
            _handler = handler;
        }

        virtual void OnComplete() OVERRIDE;

        IMPLEMENT_REFCOUNTING(CompletionHandler);
    };
}
// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefSharpBrowserWrapper.h"
#include "JavascriptCallbackProxy.h"
#include "JavascriptCallbackFactory.h"

namespace CefSharp
{
    namespace Internals
    {
        void JavascriptCallbackFactory::BrowserWrapper::set(WeakReference^ browserWrapper)
        {
            _browserWrapper = browserWrapper;
        }

        IJavascriptCallback^ JavascriptCallbackFactory::Create(JavascriptCallback^ callback)
        {
            return gcnew JavascriptCallbackProxy(callback, _pendingTasks, _browserWrapper);
        }
    }
}
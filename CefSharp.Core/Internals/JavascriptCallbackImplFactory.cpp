// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefSharpBrowserWrapper.h"
#include "JavascriptCallbackImpl.h"
#include "JavascriptCallbackImplFactory.h"

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallbackImplFactory::JavascriptCallbackImplFactory(PendingTaskRepository<JavascriptResponse^>^ pendingTasks)
            :_pendingTasks(pendingTasks)
        {
        }

        void JavascriptCallbackImplFactory::BrowserWrapper::set(CefSharpBrowserWrapper^ browserWrapper)
        {
            _browserWrapper = gcnew WeakReference(browserWrapper);
        }

        IJavascriptCallback^ JavascriptCallbackImplFactory::Create(JavascriptCallback^ callback)
        {
            return gcnew JavascriptCallbackImpl(callback, _pendingTasks, _browserWrapper);
        }
    }
}
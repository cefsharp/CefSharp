// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace System;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefSharpBrowserWrapper;

        private ref class JavascriptCallbackImplFactory : public IJavascriptCallbackFactory
        {
        private:
            WeakReference^ _browserWrapper;
            PendingTaskRepository<JavascriptResponse^>^ _pendingTasks;
        public:
            JavascriptCallbackImplFactory(PendingTaskRepository<JavascriptResponse^>^ pendingTasks);

            property CefSharpBrowserWrapper^ BrowserWrapper
            {
                void set(CefSharpBrowserWrapper^ browserWrapper);
            };

            virtual IJavascriptCallback^ Create(JavascriptCallback^ callback);
        };
    }
}
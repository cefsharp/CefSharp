// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"

using namespace System::Threading;

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallback^ JavascriptCallbackRegistry::Register(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value)
        {
            auto newId = Interlocked::Increment(_lastId);
            _callbacks.emplace(newId, new JavascriptCallbackHolder(context, value));

            auto result = gcnew JavascriptCallback();
            result->Id = newId;
            result->BrowserId = _browserId;
            return result;
        }

        bool JavascriptCallbackRegistry::Execute(int64 id, const CefV8ValueList& arguments, CefRefPtr<CefV8Value> &result, CefRefPtr<CefV8Exception> &exception)
        {
            auto success = false;
            if (_callbacks.count(id) == 1)
            {
                auto callback = _callbacks[id];
                success = callback->Execute(arguments, result, exception);
            }
            return success;
        }

        void JavascriptCallbackRegistry::Deregister(int64 id)
        {
            if (_callbacks.count(id) == 1)
            {
                _callbacks.erase(id);
            }
        }
    }
}
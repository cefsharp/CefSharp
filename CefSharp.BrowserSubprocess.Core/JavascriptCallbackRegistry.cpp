// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"

using namespace System::Threading;
using namespace CefSharp::BrowserSubprocess;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        JavascriptCallback^ JavascriptCallbackRegistry::Register(const CefRefPtr<CefV8Context>& context, const CefRefPtr<CefV8Value>& value)
        {
            Int64 newId = Interlocked::Increment(_lastId);
            JavascriptCallbackWrapper^ wrapper = gcnew JavascriptCallbackWrapper(value, context);
            _callbacks->TryAdd(newId, wrapper);

            auto result = gcnew JavascriptCallback();
            result->Id = newId;
            result->BrowserId = _browserId;
            result->FrameId = StringUtils::ToClr(context->GetFrame()->GetIdentifier());
            return result;
        }

        JavascriptCallbackWrapper^ JavascriptCallbackRegistry::FindWrapper(int64_t id)
        {
            JavascriptCallbackWrapper^ callback;
            _callbacks->TryGetValue(id, callback);
            return callback;
        }

        void JavascriptCallbackRegistry::Deregister(Int64 id)
        {
            JavascriptCallbackWrapper^ callback;
            if (_callbacks->TryRemove(id, callback))
            {
                delete callback;
            }
        }

    }
}

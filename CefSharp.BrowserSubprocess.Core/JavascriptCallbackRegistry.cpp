// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"

using namespace System::Threading;

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallback^ JavascriptCallbackRegistry::Register(const CefRefPtr<CefV8Context>& context, const CefRefPtr<CefV8Value>& value)
        {
            Int64 newId = Interlocked::Increment(_lastId);
            JavascriptCallbackWrapper^ wrapper = gcnew JavascriptCallbackWrapper(value, context);
            _callbacks->TryAdd(newId, wrapper);

            auto result = gcnew JavascriptCallback();
            result->Id = newId;
            result->BrowserId = _browserId;
            result->FrameId = -1;

            auto frame = context->GetFrame();

            if (frame.get() && frame->IsValid())
            {
                //Issue https://bitbucket.org/chromiumembedded/cef/issues/2687/cefframe-getidentifier-differs-between
                //prevents callbacks from working properly
                //As a hack to get callbacks working we'll only return an Id for the main frame
                //https://github.com/cefsharp/CefSharp/issues/2743#issuecomment-502566136
                if (frame->IsMain())
                {
                    result->FrameId = frame->GetIdentifier();
                }
            }
            return result;
        }

        JavascriptCallbackWrapper^ JavascriptCallbackRegistry::FindWrapper(int64 id)
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

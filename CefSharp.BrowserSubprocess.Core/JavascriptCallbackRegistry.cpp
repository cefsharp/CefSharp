// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "JavascriptCallbackRegistry.h"

using namespace System::Threading;

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallbackDto^ JavascriptCallbackRegistry::CreateWrapper(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value)
        {
            auto newId = Interlocked::Increment(lastId);
            auto wrapper = gcnew JavascriptCallbackWrapper(value, context);
            callbacks->Add(newId, wrapper);

            auto result = gcnew JavascriptCallbackDto();
            result->Id = newId;
            result->BrowserId = browserId;
            return result;
        }

        JavascriptResponse^ JavascriptCallbackRegistry::Execute(Int64 id, array<Object^>^ params)
        {
            JavascriptResponse^ result = nullptr;
            if (callbacks->ContainsKey(id))
            {
                result = callbacks[id]->Execute(params);
            }
            return result;
        }

        void JavascriptCallbackRegistry::RemoveWrapper(Int64 id)
        {
            if (callbacks->ContainsKey(id))
            {
                auto callback = callbacks[id];
                callbacks->Remove(id);
                delete callback;
            }
        }

        JavascriptCallbackRegistry::~JavascriptCallbackRegistry()
        {
            for each (auto callback in callbacks)
            {
                delete callback.Value;
            }
            callbacks->Clear();
        }
    }
}
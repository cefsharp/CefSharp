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
            auto newId = Interlocked::Increment(_lastId);
            auto wrapper = gcnew JavascriptCallbackWrapper(value, context);
            _callbacks->Add(newId, wrapper);

            auto result = gcnew JavascriptCallbackDto();
            result->Id = newId;
            result->BrowserId = _browserId;
            return result;
        }

        JavascriptResponse^ JavascriptCallbackRegistry::Execute(Int64 id, array<Object^>^ params)
        {
            JavascriptResponse^ result = nullptr;
            if (_callbacks->ContainsKey(id))
            {
                result = _callbacks[id]->Execute(params);
            }
            return result;
        }

        void JavascriptCallbackRegistry::RemoveWrapper(Int64 id)
        {
            if (_callbacks->ContainsKey(id))
            {
                auto callback = _callbacks[id];
                _callbacks->Remove(id);
                delete callback;
            }
        }

        JavascriptCallbackRegistry::~JavascriptCallbackRegistry()
        {
            for each (auto callback in _callbacks)
            {
                delete callback.Value;
            }
            _callbacks->Clear();
        }
    }
}
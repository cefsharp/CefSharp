// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
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
            JavascriptCallbackWrapper^ wrapper = gcnew JavascriptCallbackWrapper(value, context);
            Int64 newId = _callbacks->RegisterObject(wrapper);

            auto result = gcnew JavascriptCallback();
            result->Id = newId;
            result->BrowserId = _browserId;
            return result;
        }

        JavascriptCallbackWrapper^ JavascriptCallbackRegistry::FindWrapper(int64 id)
        {
            JavascriptCallbackWrapper^ callback;
            _callbacks->TryGetObject(id, callback);
            return callback;
        }

        void JavascriptCallbackRegistry::Deregister(Int64 id)
        {
            JavascriptCallbackWrapper^ callback;
            if(_callbacks->TryRemoveObject(id, callback))
            {
                delete callback;
            }
        }

    }
}
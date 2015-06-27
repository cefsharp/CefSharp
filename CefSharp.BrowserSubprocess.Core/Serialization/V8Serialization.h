// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

namespace CefSharp
{
    namespace Internals
    {
        ref class JavascriptCallbackRegistry;

        namespace Serialization
        {
            //Functions to sserialize data to be sent to the browser process.

            //Serializes a V8 structure into a given index of a CefListValue or CefDictionaryValue
            //JavascriptCallbackRegistry should be passed to save V8Values with function types
            template<typename TList, typename TIndex>
            void SerializeV8Object(CefRefPtr<CefV8Value> value, CefRefPtr<TList> list, TIndex index, JavascriptCallbackRegistry^ callbackRegistry);
        }
    }
}
// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_values.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            //separate file because it's needed in the subprocess too at the moment

            //Deserializes data into Object from a given index of a CefListValue or CefDictionaryValue
            //IJavascriptCallbackFactory implementation should be passed to allow creation of valid javascript callbacks
            template<typename TList, typename TIndex>
            Object^ DeserializeObject(const CefRefPtr<TList>& list, TIndex index, IJavascriptCallbackFactory^ javascriptCallbackFactory);

            //Converts CefTime to DateTime
            DateTime ConvertCefTimeToDateTime(CefTime time);

            template Object^ DeserializeObject(const CefRefPtr<CefListValue>& list, int index, IJavascriptCallbackFactory^ javascriptCallbackFactory);
            template Object^ DeserializeObject(const CefRefPtr<CefDictionaryValue>& list, CefString index, IJavascriptCallbackFactory^ javascriptCallbackFactory);
        }
    }
}
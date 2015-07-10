// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "TypeUtils.h"
#include "JavascriptCallbackWrapper.h"
#include "Serialization/V8Serialization.h"

using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        CefRefPtr<CefV8Value> JavascriptCallbackWrapper::GetValue()
        {
            return _value.get();
        }

        CefRefPtr<CefV8Context> JavascriptCallbackWrapper::GetContext()
        {
            return _context.get();
        }
    }
}

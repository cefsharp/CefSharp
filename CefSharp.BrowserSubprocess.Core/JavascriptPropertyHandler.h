// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

namespace CefSharp
{
    private class JavascriptPropertyHandler : CefV8Accessor
    {
        virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
            CefString& exception) override
        {
            // TODO: implement.
            return false;
        }

        virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
            CefString& exception) override
        {
            // TODO: implement
            return false;
        }

        IMPLEMENT_REFCOUNTING(JavascriptPropertyHandler)
    };
}

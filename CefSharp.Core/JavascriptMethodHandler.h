// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

namespace CefSharp
{
    private class BindingHandler : public CefV8Handler
    {
        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception)
        {
            // TODO: implement
            return false;
        }

        IMPLEMENT_REFCOUNTING(PropertyAccessor)
    };
}
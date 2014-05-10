// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Cef.h"
#include "ManagedCefBrowserAdapter.h"

using namespace System::Net;

namespace CefSharp
{
    Object^ ManagedCefBrowserAdapter::CallMethod(int objectId, String^ name, array<Object^>^ parameters)
    {
        Object^ result;
        if (!_javaScriptObjectRepository->TryCallMethod(objectId, name, parameters, result))
        {
            Cef::_javaScriptObjectRepository->TryCallMethod(objectId, name, parameters, result);
        }

        return result;
    }

    Object^ ManagedCefBrowserAdapter::GetProperty(int objectId, String^ name)
    {
        Object^ result;
        if (!_javaScriptObjectRepository->TryGetProperty(objectId, name, result))
        {
            Cef::_javaScriptObjectRepository->TryGetProperty(objectId, name, result);
        }

        return result;
    }

    void ManagedCefBrowserAdapter::SetProperty(int objectId, String^ name, Object^ value)
    {
        if (!_javaScriptObjectRepository->TrySetProperty(objectId, name, value))
        {
            Cef::_javaScriptObjectRepository->TrySetProperty(objectId, name, value);
        }
    }
}
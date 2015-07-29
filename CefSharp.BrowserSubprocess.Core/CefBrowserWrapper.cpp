// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"

namespace CefSharp
{
    JavascriptRootObjectWrapper^ CefBrowserWrapper::JavascriptRootObjectWrapper::get()
    {
        return _javascriptRootObjectWrapper;
    }

    void CefBrowserWrapper::JavascriptRootObjectWrapper::set(CefSharp::JavascriptRootObjectWrapper^ value)
    {
        _javascriptRootObjectWrapper = value;
        if (_javascriptRootObjectWrapper != nullptr)
        {
            _javascriptRootObjectWrapper->CallbackRegistry = _callbackRegistry;
        }
    }

    JavascriptAsyncRootObjectWrapper^ CefBrowserWrapper::JavascriptAsyncRootObjectWrapper::get()
    {
        return _javascriptAsyncRootObjectWrapper;
    }

    void CefBrowserWrapper::JavascriptAsyncRootObjectWrapper::set(CefSharp::Internals::Async::JavascriptAsyncRootObjectWrapper^ value)
    {
        _javascriptAsyncRootObjectWrapper = value;
        if (_javascriptAsyncRootObjectWrapper != nullptr)
        {
            _javascriptAsyncRootObjectWrapper->CallbackRegistry = _callbackRegistry;
            _javascriptAsyncRootObjectWrapper->MethodCallbackSave = gcnew Func<JavascriptAsyncMethodCallback^, int64>(this, &CefBrowserWrapper::SaveMethodCallback);
        }
    }

    JavascriptCallbackRegistry^ CefBrowserWrapper::CallbackRegistry::get()
    {
        return _callbackRegistry;
    }

    int64 CefBrowserWrapper::SaveMethodCallback(JavascriptAsyncMethodCallback^ callback)
    {
        auto callbackId = Interlocked::Increment(_lastCallback);
        _methodCallbacks->Add(callbackId, callback);
        return callbackId;
    }

    bool CefBrowserWrapper::TryGetAndRemoveMethodCallback(int64 id, JavascriptAsyncMethodCallback^% callback)
    {
        bool result = false;
        if (result = _methodCallbacks->TryGetValue(id, callback))
        {
            _methodCallbacks->Remove(id);
        }
        return result;
    }
}
// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"

namespace CefSharp
{
    CefBrowserWrapper::CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
    {
        _cefBrowser = cefBrowser;
        BrowserId = cefBrowser->GetIdentifier();
        IsPopup = cefBrowser->IsPopup();
    }

    JavascriptResponse^ CefBrowserWrapper::EvaluateScriptInContext(CefRefPtr<CefV8Context> context, CefString script)
    {
        CefRefPtr<CefV8Value> result;
        CefRefPtr<CefV8Exception> exception;
        JavascriptResponse^ response = gcnew JavascriptResponse();

        response->Success = context->Eval(script, result, exception);
        if (response->Success)
        {
            response->Result = TypeUtils::ConvertFromCef(result);
        }
        else if (exception.get())
        {
            response->Message = StringUtils::ToClr(exception->GetMessage());
        }

        return response;
    }

    void CefBrowserWrapper::DoDispose(bool disposing)
    {
        _cefBrowser = nullptr;
        DisposableResource::DoDispose(disposing);
    }

    JavascriptResponse^ CefBrowserWrapper::DoEvaluateScript(System::Int64 frameId, String^ script)
    {
        auto frame = _cefBrowser->GetFrame(frameId);
        CefRefPtr<CefV8Context> context = frame->GetV8Context();

        if (context.get() && context->Enter())
        {
            try
            {
                return EvaluateScriptInContext(context, StringUtils::ToNative(script));
            }
            finally
            {
                context->Exit();
            }
        }

        return nullptr;
    }

    CefBrowserWrapper::~CefBrowserWrapper()
    {
        _cefBrowser = nullptr;
    }
}
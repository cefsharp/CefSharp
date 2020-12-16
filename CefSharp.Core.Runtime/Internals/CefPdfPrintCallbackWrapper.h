// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_browser.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefPdfPrintCallbackWrapper : public CefPdfPrintCallback
        {
        private:
            gcroot<IPrintToPdfCallback^> _callback;

        public:
            CefPdfPrintCallbackWrapper(IPrintToPdfCallback^ callback)
                :_callback(callback)
            {

            }

            ~CefPdfPrintCallbackWrapper()
            {
                delete _callback;
                _callback = nullptr;
            }

            virtual void OnPdfPrintFinished(const CefString& path, bool ok) OVERRIDE
            {
                if (static_cast<IPrintToPdfCallback^>(_callback) != nullptr)
                {
                    _callback->OnPdfPrintFinished(StringUtils::ToClr(path), ok);
                }
            }

            IMPLEMENT_REFCOUNTING(CefPdfPrintCallbackWrapper);
        };
    }
}
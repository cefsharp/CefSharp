// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_browser.h"

namespace CefSharp
{
    namespace Internals
    {
        public class CefPdfPrintCallbackWrapper : public virtual CefPdfPrintCallback
        {
        private:
            gcroot<IPrintToPdfCallback^> _callback;
        public:
            CefPdfPrintCallbackWrapper(IPrintToPdfCallback^ callback)
                :_callback(callback)
            {
                
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
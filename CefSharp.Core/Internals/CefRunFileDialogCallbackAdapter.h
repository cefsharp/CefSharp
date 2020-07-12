// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_browser.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefRunFileDialogCallbackAdapter : public CefRunFileDialogCallback
        {
        private:
            gcroot<IRunFileDialogCallback^> _callback;

        public:
            CefRunFileDialogCallbackAdapter(IRunFileDialogCallback^ callback) :
                _callback(callback)
            {

            }

            ~CefRunFileDialogCallbackAdapter()
            {
                delete _callback;
                _callback = nullptr;
            }

            virtual void OnFileDialogDismissed(int selectedAcceptFilter, const std::vector<CefString>& filePaths) OVERRIDE
            {
                if (static_cast<IRunFileDialogCallback^>(_callback) != nullptr)
                {
                    _callback->OnFileDialogDismissed(selectedAcceptFilter, StringUtils::ToClr(filePaths));
                }
            }

            IMPLEMENT_REFCOUNTING(CefRunFileDialogCallbackAdapter);
        };
    }
}


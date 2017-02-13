// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefRegisterCdmCallbackWrapper : public CefRegisterCdmCallback
        {
        private:
            gcroot<TaskCompletionSource<CdmRegistration^>^> _taskCompletionSource;
            bool hasData;

        public:
            CefRegisterCdmCallbackWrapper()
            {
                _taskCompletionSource = gcnew TaskCompletionSource<CdmRegistration^>();
                hasData = false;
            }

            ~CefRegisterCdmCallbackWrapper()
            {
                if (hasData == false)
                {
                    //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                    TaskExtensions::TrySetResultAsync<CdmRegistration^>(_taskCompletionSource, nullptr);
                }
                _taskCompletionSource = nullptr;
            }

            /// <summary>
            /// Method that will be called when CDM registration is complete. |result|
            /// will be CEF_CDM_REGISTRATION_ERROR_NONE if registration completed
            /// successfully. Otherwise, |result| and |error_message| will contain
            /// additional information about why registration failed.
            /// </summary>
            virtual void OnCdmRegistrationComplete(cef_cdm_registration_error_t result,
                const CefString& error_message) OVERRIDE
            {
                auto r = gcnew CdmRegistration();

                r->ErrorCode = (CdmRegistrationErrorCode)result;
                r->ErrorMessage = StringUtils::ToClr(error_message);

                //Set the result on the ThreadPool so the Task continuation is not run on the CEF UI Thread
                TaskExtensions::TrySetResultAsync<CdmRegistration^>(_taskCompletionSource, r);

                hasData = true;
            }

            Task<CdmRegistration^>^ GetTask()
            {
                return _taskCompletionSource->Task;
            }

            IMPLEMENT_REFCOUNTING(CefRegisterCdmCallbackWrapper)
        };
    }
}
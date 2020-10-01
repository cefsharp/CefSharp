// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefRegisterCdmCallbackAdapter : public CefRegisterCdmCallback
        {
        private:
            gcroot<IRegisterCdmCallback^> _callback;

        public:
            CefRegisterCdmCallbackAdapter(IRegisterCdmCallback^ callback)
            {
                _callback = callback;
            }

            ~CefRegisterCdmCallbackAdapter()
            {
                delete _callback;
                _callback = nullptr;
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
                auto r = gcnew CdmRegistration((CdmRegistrationErrorCode)result, StringUtils::ToClr(error_message));

                _callback->OnRegistrationComplete(r);
            }

            IMPLEMENT_REFCOUNTING(CefRegisterCdmCallbackAdapter);
        };
    }
}

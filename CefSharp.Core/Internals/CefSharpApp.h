// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_app.h"
#include "CefSettings.h"
#include "SchemeHandlerWrapper.h"

namespace CefSharp
{
    class CefSharpApp : public CefApp
    {
        gcroot<CefSettingsWrapper^> _cefSettings;

    public:
        CefSharpApp(CefSettingsWrapper^ cefSettings) :
            _cefSettings(cefSettings)
        {
        }

        virtual void OnRegisterCustomSchemes(CefRefPtr<CefSchemeRegistrar> registrar) OVERRIDE
        {
            for each (CefCustomScheme^ cefCustomScheme in _cefSettings->CefCustomSchemes)
            {
                // TODO: Causes an "assertion failed" error here: DCHECK_EQ(CefCallbackCToCpp::DebugObjCt, 0)
                // when the process is shutting down.

                // TOOD: Consider adding error handling here. But where do we report any errors that may have occurred?
                registrar->AddCustomScheme(StringUtils::ToNative(cefCustomScheme->SchemeName), cefCustomScheme->IsStandard, false, false);
            }
        };

        void CompleteSchemeRegistrations()
        {
            // TOOD: Consider adding error handling here. But where do we report any errors that may have occurred?
            for each (CefCustomScheme^ cefCustomScheme in _cefSettings->CefCustomSchemes)
            {
                auto domainName = cefCustomScheme->DomainName ? cefCustomScheme->DomainName : String::Empty;

                CefRefPtr<CefSchemeHandlerFactory> wrapper = new SchemeHandlerFactoryWrapper(cefCustomScheme->SchemeHandlerFactory);
                CefRegisterSchemeHandlerFactory(StringUtils::ToNative(cefCustomScheme->SchemeName), StringUtils::ToNative(domainName), wrapper);
            }
        };

        IMPLEMENT_REFCOUNTING(CefSharpApp)
    };
}
// Copyright � 2010-2014 The CefSharp Authors. All rights reserved.
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
        gcroot<CefSettings^> _cefSettings;
        gcroot<IDictionary<String^, String^>^> _cefCommandLineArgs;

    public:
        CefSharpApp(CefSettings^ cefSettings) :
            _cefSettings(cefSettings),
            _cefCommandLineArgs(gcnew Dictionary<String^, String^>())
        {
        }

        CefSharpApp(CefSettings^ cefSettings, IDictionary<String^, String^>^ commandLineArgs) :
            _cefSettings(cefSettings)
        {
            if(commandLineArgs != nullptr)
                _cefCommandLineArgs = commandLineArgs;
            else
                _cefCommandLineArgs = gcnew Dictionary<String^, String^>();
        }

        ~CefSharpApp()
        {
            _cefSettings = nullptr;
            _cefCommandLineArgs = nullptr;
        }
        
        virtual void OnBeforeCommandLineProcessing(const CefString& process_type, CefRefPtr<CefCommandLine> command_line) OVERRIDE
        {
            if(_cefCommandLineArgs->Count == 0)
                return;

            auto enumerator = _cefCommandLineArgs->GetEnumerator();
            auto commandLine = command_line.get();

            // Not clear what should happen if we 
            // * already have some command line flags given (is this possible? Perhaps from globalCommandLine)
            // * have no flags given (-> call SetProgramm() with first argument?)

            while(enumerator->MoveNext())
            {
                CefString name = StringUtils::ToNative(enumerator->Current.Key);
                CefString value = StringUtils::ToNative(enumerator->Current.Value);

                if(!commandLine->HasSwitch(name))
                    commandLine->AppendSwitchWithValue(name, value);
            }
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

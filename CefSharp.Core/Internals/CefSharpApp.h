// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_app.h"
#include "CefSettings.h"

namespace CefSharp
{
    private class CefSharpApp : public CefApp,
        public CefBrowserProcessHandler
    {
        gcroot<CefSettings^> _cefSettings;
        gcroot<Action^> _onContextInitialized;

    public:
        CefSharpApp(CefSettings^ cefSettings, Action^ onContextInitialized) :
            _cefSettings(cefSettings),
            _onContextInitialized(onContextInitialized)
        {
        }

        ~CefSharpApp()
        {
            _cefSettings = nullptr;
            _onContextInitialized = nullptr;
        }

        virtual CefRefPtr<CefBrowserProcessHandler> GetBrowserProcessHandler() OVERRIDE
        {
            return this;
        }

        virtual void OnContextInitialized() OVERRIDE
        {
            if (!Object::ReferenceEquals(_onContextInitialized, nullptr))
            {
                _onContextInitialized->Invoke();
            }
        }

        virtual void OnBeforeChildProcessLaunch(CefRefPtr<CefCommandLine> commandLine) OVERRIDE
        {
            if (CefSharpSettings::WcfEnabled)
            {
                commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::WcfEnabledArgument));
            }

            if (_cefSettings->_cefCustomSchemes->Count > 0)
            {
                String^ argument = "=";

                for each(CefCustomScheme^ scheme in _cefSettings->CefCustomSchemes)
                {
                    argument += scheme->SchemeName + "|";
                    argument += (scheme->IsStandard ? "T" : "F") + "|";
                    argument += (scheme->IsLocal ? "T" : "F") + "|";
                    argument += (scheme->IsDisplayIsolated ? "T" : "F") + ";";
                }

                argument = argument->TrimEnd(';');

                commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::CustomSchemeArgument + argument));
            }

            if (_cefSettings->FocusedNodeChangedEnabled)
            {
                commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::FocusedNodeChangedEnabledArgument));
            }
        }
        
        virtual void OnBeforeCommandLineProcessing(const CefString& process_type, CefRefPtr<CefCommandLine> command_line) OVERRIDE
        {
            if(_cefSettings->CefCommandLineArgs->Count > 0)
            {
                auto commandLine = command_line.get();

                // Not clear what should happen if we 
                // * already have some command line flags given (is this possible? Perhaps from globalCommandLine)
                // * have no flags given (-> call SetProgramm() with first argument?)

                for each(KeyValuePair<String^, String^>^ kvp in _cefSettings->CefCommandLineArgs)
                {
                    CefString name = StringUtils::ToNative(kvp->Key);
                    CefString value = StringUtils::ToNative(kvp->Value);

                    // Right now the command line args handed to the application (global command line) have higher
                    // precedence than command line args provided by the app
                    if(!commandLine->HasSwitch(name))
                    {
                        commandLine->AppendSwitchWithValue(name, value);
                    }
                }
            }
        }

        virtual void OnRegisterCustomSchemes(CefRefPtr<CefSchemeRegistrar> registrar) OVERRIDE
        {
            for each (CefCustomScheme^ cefCustomScheme in _cefSettings->CefCustomSchemes)
            {
                // TOOD: Consider adding error handling here. But where do we report any errors that may have occurred?
                registrar->AddCustomScheme(StringUtils::ToNative(cefCustomScheme->SchemeName), cefCustomScheme->IsStandard, cefCustomScheme->IsLocal, cefCustomScheme->IsDisplayIsolated);
            }
        };

        virtual void OnRenderProcessThreadCreated(CefRefPtr<CefListValue> extraInfo) OVERRIDE
        {
            auto extensionList = CefListValue::Create();

            auto i = 0;
            for each(CefExtension^ cefExtension in _cefSettings->Extensions)
            {
                auto ext = CefListValue::Create();
                ext->SetString(0, StringUtils::ToNative(cefExtension->Name));
                ext->SetString(1, StringUtils::ToNative(cefExtension->JavascriptCode));
                extensionList->SetList(i++, ext);
            }

            extraInfo->SetList(0, extensionList);
        }

        IMPLEMENT_REFCOUNTING(CefSharpApp)
    };
}

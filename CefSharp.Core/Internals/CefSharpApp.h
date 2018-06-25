// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        gcroot<IBrowserProcessHandler^> _browserProcessHandler;

    public:
        CefSharpApp(CefSettings^ cefSettings, IBrowserProcessHandler^ browserProcessHandler) :
            _cefSettings(cefSettings),
            _browserProcessHandler(browserProcessHandler)
        {
            if (cefSettings->ExternalMessagePump && Object::ReferenceEquals(_browserProcessHandler, nullptr))
            {
                throw gcnew Exception("browserProcessHandler cannot be null when using cefSettings.ExternalMessagePump");
            }
        }

        ~CefSharpApp()
        {
            _cefSettings = nullptr;
            delete _browserProcessHandler;
            _browserProcessHandler = nullptr;
        }

        virtual CefRefPtr<CefBrowserProcessHandler> GetBrowserProcessHandler() OVERRIDE
        {
            return this;
        }

        virtual void OnContextInitialized() OVERRIDE
        {
            if (!Object::ReferenceEquals(_browserProcessHandler, nullptr))
            {
                _browserProcessHandler->OnContextInitialized();
            }
        }

        virtual void OnScheduleMessagePumpWork(int64 delay_ms)  OVERRIDE
        {
            _browserProcessHandler->OnScheduleMessagePumpWork(delay_ms);
        }

        virtual void OnBeforeChildProcessLaunch(CefRefPtr<CefCommandLine> commandLine) OVERRIDE
        {
            if (CefSharpSettings::WcfEnabled)
            {
                commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::WcfEnabledArgument));
            }

            if (CefSharpSettings::SubprocessExitIfParentProcessClosed)
            {
                commandLine->AppendSwitch(StringUtils::ToNative(CefSharpArguments::ExitIfParentProcessClosed));
            }

            //ChannelId was removed in https://bitbucket.org/chromiumembedded/cef/issues/1912/notreached-in-logchannelidandcookiestores
            //We need to know the process Id to establish WCF communication and for monitoring of parent process exit
            commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::HostProcessIdArgument + "=" + Process::GetCurrentProcess()->Id));

            if (_cefSettings->_cefCustomSchemes->Count > 0)
            {
                String^ argument = "=";

                for each(CefCustomScheme^ scheme in _cefSettings->CefCustomSchemes)
                {
                    argument += scheme->SchemeName + "|";
                    argument += (scheme->IsStandard ? "T" : "F") + "|";
                    argument += (scheme->IsLocal ? "T" : "F") + "|";
                    argument += (scheme->IsDisplayIsolated ? "T" : "F") + "|";
                    argument += (scheme->IsSecure ? "T" : "F") + "|";
                    argument += (scheme->IsCorsEnabled ? "T" : "F") + "|";
                    argument += (scheme->IsCSPBypassing ? "T" : "F") + ";";
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

                    if (kvp->Key == "disable-features")
                    {
                        //Temp workaround so we can set the disable-features command line argument
                        // See https://github.com/cefsharp/CefSharp/issues/2408
                        commandLine->AppendSwitchWithValue(name, value);
                    }
                    // Right now the command line args handed to the application (global command line) have higher
                    // precedence than command line args provided by the app
                    else if(!commandLine->HasSwitch(name))
                    {
                        commandLine->AppendSwitchWithValue(name, value);
                    }
                }
            }
        }

        virtual void OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar) OVERRIDE
        {
            for each (CefCustomScheme^ scheme in _cefSettings->CefCustomSchemes)
            {
                auto success = registrar->AddCustomScheme(StringUtils::ToNative(scheme->SchemeName), scheme->IsStandard, scheme->IsLocal, scheme->IsDisplayIsolated, scheme->IsSecure, scheme->IsCorsEnabled, scheme->IsCSPBypassing);

                if (!success)
                {
                    String^ msg = "CefSchemeRegistrar::AddCustomScheme failed for schemeName:" + scheme->SchemeName;
                    LOG(ERROR) << StringUtils::ToNative(msg).ToString();
                }
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

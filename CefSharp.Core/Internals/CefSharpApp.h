// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_app.h"
#include "include\cef_scheme.h"

#include "CefSettingsBase.h"
#include "CefSchemeHandlerFactoryAdapter.h"
#include "Internals\CefSchemeRegistrarWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefSharpApp : public CefApp,
            public CefBrowserProcessHandler
        {
            gcroot<CefSettingsBase^> _cefSettings;
            gcroot<IApp^> _app;

        public:
            CefSharpApp(CefSettingsBase^ cefSettings, IApp^ app) :
                _cefSettings(cefSettings),
                _app(app)
            {
                auto isMissingHandler = Object::ReferenceEquals(app, nullptr) || Object::ReferenceEquals(app->BrowserProcessHandler, nullptr);
                if (cefSettings->ExternalMessagePump && isMissingHandler)
                {
                    throw gcnew Exception("browserProcessHandler cannot be null when using cefSettings.ExternalMessagePump");
                }
            }

            ~CefSharpApp()
            {
                _cefSettings = nullptr;
                delete _app;
                _app = nullptr;
            }

            virtual CefRefPtr<CefBrowserProcessHandler> GetBrowserProcessHandler() OVERRIDE
            {
                return this;
            }

            virtual void OnContextInitialized() OVERRIDE
            {
                if (!Object::ReferenceEquals(_app, nullptr) && !Object::ReferenceEquals(_app->BrowserProcessHandler, nullptr))
                {
                    _app->BrowserProcessHandler->OnContextInitialized();
                }

                //CefRegisterSchemeHandlerFactory requires access to the Global CefRequestContext
                for each (CefCustomScheme^ cefCustomScheme in _cefSettings->CefCustomSchemes)
                {
                    if (!Object::ReferenceEquals(cefCustomScheme->SchemeHandlerFactory, nullptr))
                    {
                        auto domainName = cefCustomScheme->DomainName ? cefCustomScheme->DomainName : String::Empty;

                        CefRefPtr<CefSchemeHandlerFactory> wrapper = new CefSchemeHandlerFactoryAdapter(cefCustomScheme->SchemeHandlerFactory);
                        CefRegisterSchemeHandlerFactory(StringUtils::ToNative(cefCustomScheme->SchemeName), StringUtils::ToNative(domainName), wrapper);
                    }
                }
            }

            virtual void OnScheduleMessagePumpWork(int64 delay_ms)  OVERRIDE
            {
                //We rely on previous checks to make sure _app and _app->BrowserProcessHandler aren't null
                _app->BrowserProcessHandler->OnScheduleMessagePumpWork(delay_ms);
            }

            virtual void OnBeforeChildProcessLaunch(CefRefPtr<CefCommandLine> commandLine) OVERRIDE
            {
#ifndef NETCOREAPP
                if (CefSharpSettings::WcfEnabled)
                {
                    commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::WcfEnabledArgument));
                }
#endif

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
                    bool hasCustomScheme = false;
                    auto registeredSchemes = gcnew List<String^>();

                    for each(CefCustomScheme^ scheme in _cefSettings->CefCustomSchemes)
                    {
                        //We don't need to register http or https in the render process
                        if (scheme->SchemeName == "http" ||
                            scheme->SchemeName == "https")
                        {
                            continue;
                        }

                        //We've already registered this scheme name
                        if (registeredSchemes->Contains(scheme->SchemeName))
                        {
                            continue;
                        }

                        hasCustomScheme = true;

                        registeredSchemes->Add(scheme->SchemeName);

                        argument += scheme->SchemeName + "|";
                        argument += ((int)scheme->Options).ToString() + ";";
                    }

                    if (hasCustomScheme)
                    {
                        argument = argument->TrimEnd(';');

                        commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::CustomSchemeArgument + argument));
                    }
                }

                if (CefSharpSettings::FocusedNodeChangedEnabled)
                {
                    commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::FocusedNodeChangedEnabledArgument));
                }
            }

            virtual void OnBeforeCommandLineProcessing(const CefString& process_type, CefRefPtr<CefCommandLine> command_line) OVERRIDE
            {
                if (CefSharpSettings::Proxy != nullptr && !_cefSettings->CommandLineArgsDisabled)
                {
                    command_line->AppendSwitchWithValue("proxy-server", StringUtils::ToNative(CefSharpSettings::Proxy->IP + ":" + CefSharpSettings::Proxy->Port));

                    if (!String::IsNullOrEmpty(CefSharpSettings::Proxy->BypassList))
                    {
                        command_line->AppendSwitchWithValue("proxy-bypass-list", StringUtils::ToNative(CefSharpSettings::Proxy->BypassList));
                    }
                }

                if (_cefSettings->CefCommandLineArgs->Count > 0)
                {
                    auto commandLine = command_line.get();

                    // Not clear what should happen if we 
                    // * already have some command line flags given (is this possible? Perhaps from globalCommandLine)
                    // * have no flags given (-> call SetProgramm() with first argument?)

                    for each(KeyValuePair<String^, String^>^ kvp in _cefSettings->CefCommandLineArgs)
                    {
                        CefString name = StringUtils::ToNative(kvp->Key);
                        CefString value = StringUtils::ToNative(kvp->Value);

                        if (kvp->Key == "disable-features" || kvp->Key == "enable-features")
                        {
                            //Temp workaround so we can set the disable-features/enable-features command line argument
                            // See https://github.com/cefsharp/CefSharp/issues/2408
                            commandLine->AppendSwitchWithValue(name, value);
                        }
                        // Right now the command line args handed to the application (global command line) have higher
                        // precedence than command line args provided by the app
                        else if (!commandLine->HasSwitch(name))
                        {
                            if (String::IsNullOrEmpty(kvp->Value))
                            {
                                commandLine->AppendSwitch(name);
                            }
                            else
                            {
                                commandLine->AppendSwitchWithValue(name, value);
                            }
                        }
                    }
                }
            }

            virtual void OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar) OVERRIDE
            {
                if (!Object::ReferenceEquals(_app, nullptr))
                {
                    CefSchemeRegistrarWrapper wrapper(registrar);

                    _app->OnRegisterCustomSchemes(%wrapper);
                }
            };

            IMPLEMENT_REFCOUNTING(CefSharpApp);
        };
    }
}

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

using namespace CefSharp::Core;

namespace CefSharp
{
    namespace Internals
    {
        private class CefSharpApp : public CefApp,
            public CefBrowserProcessHandler
        {
            gcroot<IEnumerable<CefCustomScheme^>^> _customSchemes;
            gcroot<CommandLineArgDictionary^> _commandLineArgs;
            gcroot<IApp^> _app;
            bool _commandLineDisabled;
            bool _hasCustomScheme;
            gcroot<String^> _customSchemeArg;

        public:
            CefSharpApp(bool externalMessagePump, bool commandLineDisabled, CommandLineArgDictionary^ commandLineArgs, IEnumerable<CefCustomScheme^>^ customSchemes, IApp^ app) :
                _commandLineDisabled(commandLineDisabled),
                _commandLineArgs(commandLineArgs),
                _customSchemes(customSchemes),
                _app(app),
                _hasCustomScheme(false)
            {
                auto isMissingHandler = Object::ReferenceEquals(app, nullptr) || Object::ReferenceEquals(app->BrowserProcessHandler, nullptr);
                if (externalMessagePump && isMissingHandler)
                {
                    throw gcnew Exception("browserProcessHandler cannot be null when using cefSettings.ExternalMessagePump");
                }

                if (System::Linq::Enumerable::Count(customSchemes) > 0)
                {
                    String^ argument = "=";
                    auto registeredSchemes = gcnew List<String^>();

                    for each (CefCustomScheme ^ scheme in customSchemes)
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

                        _hasCustomScheme = true;

                        registeredSchemes->Add(scheme->SchemeName);

                        argument += scheme->SchemeName + "|";
                        argument += ((int)scheme->Options).ToString() + ";";
                    }

                    if (_hasCustomScheme)
                    {
                        _customSchemeArg = argument->TrimEnd(';');
                    }
                }
            }

            ~CefSharpApp()
            {
                _customSchemes = nullptr;
                _commandLineArgs = nullptr;
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

                auto customSchemes = (IEnumerable<CefCustomScheme^>^)_customSchemes;

                //CefRegisterSchemeHandlerFactory requires access to the Global CefRequestContext
                for each (CefCustomScheme^ cefCustomScheme in customSchemes)
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

                if (_hasCustomScheme)
                {
                    commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::CustomSchemeArgument + _customSchemeArg));
                }

                if (CefSharpSettings::FocusedNodeChangedEnabled)
                {
                    commandLine->AppendArgument(StringUtils::ToNative(CefSharpArguments::FocusedNodeChangedEnabledArgument));
                }
            }

            virtual void OnBeforeCommandLineProcessing(const CefString& process_type, CefRefPtr<CefCommandLine> command_line) OVERRIDE
            {
                if (CefSharpSettings::Proxy != nullptr && !_commandLineDisabled)
                {
                    command_line->AppendSwitchWithValue("proxy-server", StringUtils::ToNative(CefSharpSettings::Proxy->IP + ":" + CefSharpSettings::Proxy->Port));

                    if (!String::IsNullOrEmpty(CefSharpSettings::Proxy->BypassList))
                    {
                        command_line->AppendSwitchWithValue("proxy-bypass-list", StringUtils::ToNative(CefSharpSettings::Proxy->BypassList));
                    }
                }

                if (_commandLineArgs->Count > 0)
                {
                    auto commandLine = command_line.get();

                    // Not clear what should happen if we 
                    // * already have some command line flags given (is this possible? Perhaps from globalCommandLine)
                    // * have no flags given (-> call SetProgramm() with first argument?)

                    auto args = (CommandLineArgDictionary^)_commandLineArgs;

                    for each(KeyValuePair<String^, String^>^ kvp in args)
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

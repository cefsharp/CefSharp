// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_app.h"
#include "CefSettings.h"

using namespace CefSharp::Internals;

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

        virtual void OnBeforeChildProcessLaunch(CefRefPtr<CefCommandLine> command_line) OVERRIDE
        {
            if (CefSharpSettings::WcfEnabled)
            {
                command_line->AppendArgument(StringUtils::ToNative(CefSharpArguments::WcfEnabledArgument));
            }
        }
        
        virtual void OnBeforeCommandLineProcessing(const CefString& process_type, CefRefPtr<CefCommandLine> command_line) OVERRIDE
        {
            if(_cefSettings->CefCommandLineArgs->Count == 0)
                return;

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

            auto schemeList = CefListValue::Create();

            i = 0;
            for each(CefCustomScheme^ scheme in _cefSettings->CefCustomSchemes)
            {
                auto item = CefListValue::Create();
                item->SetString(0, StringUtils::ToNative(scheme->SchemeName));
                item->SetBool(1, scheme->IsStandard);
                item->SetBool(2, scheme->IsLocal);
                item->SetBool(3, scheme->IsDisplayIsolated);
                schemeList->SetList(i++, item);
            }

            extraInfo->SetList(1, schemeList);
        }

        IMPLEMENT_REFCOUNTING(CefSharpApp)
    };
}

#include "stdafx.h"
#include "Internals/Serialization/Primitives.h"
#include "Internals/Messaging/Messages.h"
#include "../Serialization/V8Serialization.h"
#include "JavascriptCallbackDelegate.h"
#include "../CefAppUnmanagedWrapper.h"
#include "../JavascriptCallbackRegistry.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Serialization;

        namespace Messaging
        {
            JavascriptCallbackDelegate::JavascriptCallbackDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper)
                :_appUnmanagedWrapper(appUnmanagedWrapper)
            {

            }

            bool JavascriptCallbackDelegate::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
            {
                auto handled = false;
                auto name = message->GetName();
                auto browserWrapper = _appUnmanagedWrapper->FindBrowserWrapper(browser, false);
                auto argList = message->GetArgumentList();
                if (name == kJavascriptCallback && browserWrapper != nullptr)
                {
                    auto jsCallbackId = GetInt64(argList, 0);
                    auto doneCallbackId = GetInt64(argList, 1);
                    auto paramList = argList->GetList(2);
                    auto paramCount = paramList->GetSize();
                    CefV8ValueList arguments;
                    for (auto i = 0; i < paramCount; i++)
                    {
                        arguments.push_back(DeserializeV8Object(paramList, i));
                    }

                    auto response = DoCallback(jsCallbackId, doneCallbackId, arguments, browserWrapper->GetCallbackRegistry());
                    browser->SendProcessMessage(source_process, response);

                    handled = true;
                }
                else if (name == kJavascriptCallbackDestroy && browserWrapper != nullptr)
                {
                    auto id = GetInt64(argList, 0);
                    auto callbackRegistry = browserWrapper->GetCallbackRegistry();
                    callbackRegistry->Deregister(id);
                }

                return handled;
            }

            CefRefPtr<CefProcessMessage> JavascriptCallbackDelegate::DoCallback(const int64 &jsCallbackId, const int64 &doneCallbackId, const CefV8ValueList& arguments, CefRefPtr<JavascriptCallbackRegistry> registry)
            {
                CefRefPtr<CefV8Value> result;
                CefRefPtr<CefV8Exception> exception;
                auto success = registry->Execute(jsCallbackId, arguments, result, exception);
                auto response = CefProcessMessage::Create(kJavascriptCallbackDone);
                auto argList = response->GetArgumentList();
                argList->SetBool(0, success);
                SetInt64(doneCallbackId, argList, 1);

                if (success)
                {
                    SerializeV8Object(result, argList, 3, registry);
                }
                else
                {
                    argList->SetString(3, exception->GetMessage());
                }

                return response;
            }
        }
    }
}
#include "stdafx.h"
#include "Serialization/ObjectsDeserialization.h"
#include "Internals/Messaging/Messages.h"
#include "../CefAppUnmanagedWrapper.h"
#include "JsRootObjectDelegate.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Serialization;

        namespace Messaging
        {
            JsRootObjectDelegate::JsRootObjectDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper)
                :_appUnmanagedWrapper(appUnmanagedWrapper)
            {

            }

            bool JsRootObjectDelegate::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
            {
                auto handled = false;
                auto name = message->GetName();
                auto browserWrapper = _appUnmanagedWrapper->FindBrowserWrapper(browser, false);
                if (browserWrapper != nullptr && name == kJsRootObject)
                {
                    auto argumentList = message->GetArgumentList();
                    browserWrapper->JavascriptRootObject = DeserializeJsRootObject(argumentList);
                    handled = true;
                }

                return handled;
            }
        }
    }
}
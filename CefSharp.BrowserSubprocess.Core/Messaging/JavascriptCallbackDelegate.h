#pragma once

#include "Internals/Messaging/ProcessMessageDelegate.h"

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    namespace Internals
    {
        class JavascriptCallbackRegistry;

        namespace Messaging
        {
            class JavascriptCallbackDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(JavascriptCallbackDelegate);

                CefRefPtr<CefAppUnmanagedWrapper> _appUnmanagedWrapper;

                CefRefPtr<CefProcessMessage> DoCallback(const int64 &jsCallbackId, const int64 &doneCallbackId, const CefV8ValueList& arguments, CefRefPtr<JavascriptCallbackRegistry> registry);
            public:
                JavascriptCallbackDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper);

                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
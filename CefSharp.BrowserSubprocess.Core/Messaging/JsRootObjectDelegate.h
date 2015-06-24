#pragma once

#include "Internals/Messaging/ProcessMessageDelegate.h"

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    namespace Internals
    {
        namespace Messaging
        {
            class JsRootObjectDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(JsRootObjectDelegate);

                CefRefPtr<CefAppUnmanagedWrapper> _appUnmanagedWrapper;
            public:
                JsRootObjectDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper);
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}


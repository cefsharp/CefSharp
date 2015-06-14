#pragma once

#include "Internals/Messaging/ProcessMessageDelegate.h"

namespace CefSharp
{
    class CefAppUnmanagedWrapper;

    namespace Internals
    {
        namespace Messaging
        {
            class EvaluateScriptDelegate : public ProcessMessageDelegate
            {
            private:
                DISALLOW_IMPLICIT_CONSTRUCTORS(EvaluateScriptDelegate);

                CefRefPtr<CefAppUnmanagedWrapper> _appUnmanagedWrapper;

                CefRefPtr<CefProcessMessage> EvaluateScript(int browserId, int frameId, int64 callbackId, CefString script);
                CefRefPtr<CefProcessMessage> EvaluateScriptInFrame(CefRefPtr<CefFrame> frame, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry);
                CefRefPtr<CefProcessMessage> EvaluateScriptInContext(CefRefPtr<CefV8Context> context, int64 callbackId, CefString script, JavascriptCallbackRegistry^ callbackRegistry);
            public:
                EvaluateScriptDelegate(CefRefPtr<CefAppUnmanagedWrapper> appUnmanagedWrapper);
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) override;
            };
        }
    }
}
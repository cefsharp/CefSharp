#pragma once

#include <set>

#include "include\cef_base.h"
#include "include\cef_app.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            class ProcessMessageDelegate : public virtual CefBase
            {
            public:
                virtual bool OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message) = 0;

                IMPLEMENT_REFCOUNTING(ProcessMessageDelegate);
            };

            typedef std::set<CefRefPtr<ProcessMessageDelegate>> ProcessMessageDelegateSet;
        }
    }
}
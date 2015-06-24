#pragma once

#include<include\cef_v8.h>

namespace CefSharp
{
    namespace Internals
    {
        class JavascriptCallbackHolder : public virtual CefBase
        {
        private:
            DISALLOW_IMPLICIT_CONSTRUCTORS(JavascriptCallbackHolder);

            CefRefPtr<CefV8Value> _value;
            CefRefPtr<CefV8Context> _context;
        public:
            JavascriptCallbackHolder(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value);

            bool Execute(const CefV8ValueList &parameters, CefRefPtr<CefV8Value> &result, CefRefPtr<CefV8Exception> &exception);

            IMPLEMENT_REFCOUNTING(JavascriptCallbackHolder);
        };
    }
}
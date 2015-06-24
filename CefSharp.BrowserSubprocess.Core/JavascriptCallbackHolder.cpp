#include "stdafx.h"
#include "JavascriptCallbackHolder.h"

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallbackHolder::JavascriptCallbackHolder(CefRefPtr<CefV8Context> context, CefRefPtr<CefV8Value> value)
            :_context(context), _value(value)
        {

        }

        bool JavascriptCallbackHolder::Execute(const CefV8ValueList &parameters, CefRefPtr<CefV8Value> &result, CefRefPtr<CefV8Exception> &exception)
        {
            auto success = false;
            if (_context->Enter())
            {
                result = _value->ExecuteFunctionWithContext(_context, NULL, parameters);
                if (result.get())
                {
                    success = true;
                }
                else
                {
                    exception = _value->GetException();
                }
                _context->Exit();
            }
            return success;
        }
    }
}
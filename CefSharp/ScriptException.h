#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public ref class ScriptException : public Exception
    {
        public: ScriptException()
                    : Exception()
            {}

        public: ScriptException(String^ message)
                    : Exception(message)
            {}
    };
}

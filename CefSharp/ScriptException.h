#include "stdafx.h"
#pragma once

using namespace System;
using namespace System::Runtime::Serialization;

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

        public: ScriptException(String^ message, Exception^ innerException)
                    : Exception(message, innerException)
            {}

        public: ScriptException(SerializationInfo^ info, StreamingContext context)
                    : Exception(info, context)
            {}
    };

}
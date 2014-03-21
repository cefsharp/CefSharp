using System;

namespace CefSharp
{
    public class ScriptException : Exception
    {
        public ScriptException()
            : base()
        {
        }

        public ScriptException(string message)
            : base(message)
        {
        }
    };
}

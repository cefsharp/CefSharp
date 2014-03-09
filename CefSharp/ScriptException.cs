using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

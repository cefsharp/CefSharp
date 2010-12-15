using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Tests
{
    public class BoundObject
    {
        public void EchoVoid()
        {
        }

        public bool EchoBoolean(bool arg0)
        {
            return arg0;
        }

        public bool? EchoNullableBoolean(bool? arg0)
        {
            return arg0;
        }



        public int EchoInt(int arg0)
        {
            return arg0;
        }

        public string EchoString(string arg0)
        {
            return arg0;
        }
    }
}

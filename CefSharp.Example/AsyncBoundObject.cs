using System;
using System.Threading;

namespace CefSharp.Example
{
    public class AsyncBoundObject
    {
        public void Error()
        {
            throw new Exception("This is an exception coming from C#");
        }

        public int Div(int divident, int divisor)
        {
            return divident / divisor;
        }

        public string Hello(string name)
        {
            return "Hello " + name;
        }

        public void DoSomething()
        {
            Thread.Sleep(1000);
        }
    }
}

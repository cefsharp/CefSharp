using System;

namespace CefSharp.Example
{
    public class BoundObject
    {
        public string Repeat(string str, int n)
        {
            Console.WriteLine("In bound object method");

            string result = "";
            for (int i = 0; i < n; i++)
            {
                result += str;
            }
            return result;
        }

        public void EchoVoid()
        {
        }
    }
}
using System;
using CefSharp.ModelBinding;

namespace CefSharp.Wpf.Example.Views 
{

    partial class BrowserTabView 
    {

        private class MethodInterceptionLogger : IMethodInterceptor 
        {
            public object Intercept(Func<object> originalMethod, string methodName) 
            {
                object result = originalMethod();
                System.Diagnostics.Debug.WriteLine("Called " + methodName);
                return result;
            }
        }
    }
}

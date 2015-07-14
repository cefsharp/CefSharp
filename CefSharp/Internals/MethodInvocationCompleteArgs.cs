using System;

namespace CefSharp.Internals
{
    public sealed class MethodInvocationCompleteArgs : EventArgs
    {
        public MethodInvocationResult Result { get; private set; }

        public MethodInvocationCompleteArgs(MethodInvocationResult result)
        {
            Result = result;
        }
    }
}

namespace CefSharp.Internals
{
    public sealed class MethodInvocationResult
    {
        public long? CallbackId { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public object Result { get; set; }
    }
}

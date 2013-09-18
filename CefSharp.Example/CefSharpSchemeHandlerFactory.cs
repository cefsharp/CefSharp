namespace CefSharp.Example
{
    internal class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "cefsharp";

        public ISchemeHandler Create()
        {
            return new CefSharpSchemeHandler();
        }
    }
}
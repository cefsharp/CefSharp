namespace CefSharp.Example
{
    internal class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "custom";

        public ISchemeHandler Create()
        {
            return new CefSharpSchemeHandler();
        }
    }
}
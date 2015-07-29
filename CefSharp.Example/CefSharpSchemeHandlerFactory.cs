namespace CefSharp.Example
{
    internal class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "custom";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if(schemeName == SchemeName && request.Url.EndsWith("debug.log", System.StringComparison.OrdinalIgnoreCase))
            {
                //Display the debug.log file in the browser
                return ResourceHandler.FromFileName("Debug.log", ".txt");
            }
            return new CefSharpSchemeHandler();
        }
    }
}
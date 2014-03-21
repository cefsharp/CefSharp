namespace CefSharp
{
    public class CefCustomScheme
    {
        public string SchemeName { get; set; }
        public string DomainName { get; set; }
        public bool IsStandard { get; set; }
        public ISchemeHandlerFactory SchemeHandlerFactory { get; set; }

        public CefCustomScheme()
        {
            IsStandard = true;
        }
    }
}

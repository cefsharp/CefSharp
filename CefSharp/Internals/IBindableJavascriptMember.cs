namespace CefSharp.Internals
{
    public interface IBindableJavascriptMember
    {
        void Bind(JavascriptObject owner, bool topLevel);
    }
}
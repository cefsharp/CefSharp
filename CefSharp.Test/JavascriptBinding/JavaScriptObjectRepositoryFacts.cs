using CefSharp.Internals;
using Xunit;

internal class NoNamespaceClass
{
    public SomeElseClass SomeElseClass { get; set; }
    public int Year { get; set; }

    public string GetExampleString()
    {
        return "ok";
    }
}

internal class SomeElseClass
{

}


namespace CefSharp.Test.JavascriptBinding
{  
    public class JavaScriptObjectRepositoryFacts
    {
        [Fact]
        public void CanRegisterJavascriptObjectBindWhenNamespaceIsNull()
        {
            var javascriptObjectRepository = new JavascriptObjectRepository();
            var name = nameof(NoNamespaceClass);
            javascriptObjectRepository.Register(name, new NoNamespaceClass(), false, new BindingOptions() { });
            Assert.True(javascriptObjectRepository.IsBound(name));

            var result = ((IJavascriptObjectRepositoryInternal)javascriptObjectRepository).TryCallMethod(1, "getExampleString", new object[0]);
            Assert.True(result.Success);
            Assert.Equal("ok", result.ReturnValue.ToString());
        }
    }
}

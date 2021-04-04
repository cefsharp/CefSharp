using CefSharp.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
{    /// <summary>
     /// This is more a set of integration tests than it is unit tests, for now we need to
     /// run our QUnit tests in an automated fashion and some other testing.
     /// </summary>
    //TODO: Improve Test Naming, we need a naming scheme that fits these cases that's consistent
    //(Ideally we implement consistent naming accross all test classes, though I'm open to a different
    //naming convention as these are more integration tests than unit tests).
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavaScriptObjectRepositoryFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public JavaScriptObjectRepositoryFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

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

// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System.Collections.Generic;
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
            IJavascriptObjectRepositoryInternal javascriptObjectRepository = new JavascriptObjectRepository();
            var name = nameof(NoNamespaceClass);
#if NETCOREAPP
            javascriptObjectRepository.Register(name, new NoNamespaceClass(), new BindingOptions());
#else
            javascriptObjectRepository.Register(name, new NoNamespaceClass(), false, new BindingOptions());
#endif
            Assert.True(javascriptObjectRepository.IsBound(name));

            var boundObjects = javascriptObjectRepository.GetObjects(new List<string> { name });
            Assert.Equal(1, boundObjects.Count);

            var result = javascriptObjectRepository.TryCallMethod(boundObjects[0].Id, "getExampleString", new object[0]);
            Assert.True(result.Success);
            Assert.Equal("ok", result.ReturnValue.ToString());
        }
    }
}

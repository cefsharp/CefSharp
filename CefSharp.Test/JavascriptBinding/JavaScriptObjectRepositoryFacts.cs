// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Example.ModelBinding;
using CefSharp.Internals;
using System;
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
            Assert.Single(boundObjects);

            var result = javascriptObjectRepository.TryCallMethod(boundObjects[0].Id, "getExampleString", new object[0]);
            Assert.True(result.Success);
            Assert.Equal("ok", result.ReturnValue.ToString());
        }

        [Fact]
        public void ShouldReturnErrorMessageForObjectInvalidId()
        {
            IJavascriptObjectRepositoryInternal javascriptObjectRepository = new JavascriptObjectRepository();
            
            var result = javascriptObjectRepository.TryCallMethod(100, "getExampleString", new object[0]);
            Assert.False(result.Success);
            Assert.StartsWith("Object Not Found Matching Id", result.Exception);
        }

#if !NETCOREAPP
        [Fact]
        public void CanRegisterJavascriptObjectPropertyBindWhenNamespaceIsNull()
        {
            IJavascriptObjectRepositoryInternal javascriptObjectRepository = new JavascriptObjectRepository();
            var name = nameof(NoNamespaceClass);

            BindingOptions bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                PropertyInterceptor = new PropertyInterceptorLogger()
            };
            javascriptObjectRepository.Register(name, new NoNamespaceClass(), false, bindingOptions);
            Assert.True(javascriptObjectRepository.IsBound(name));

            var boundObjects = javascriptObjectRepository.GetObjects(new List<string> { name });
            Assert.Single(boundObjects);

            object getResult, setResult = 100;
            string exception;
            NoNamespaceClass noNamespaceClass = new NoNamespaceClass();
            bool retValue = javascriptObjectRepository.TrySetProperty(boundObjects[0].Id, "year", setResult, out exception);
            Assert.True(retValue);

            retValue = javascriptObjectRepository.TryGetProperty(boundObjects[0].Id, "year", out getResult, out exception);
            Assert.True(retValue);
            Assert.Equal(100, Convert.ToInt32(getResult));
        }
#endif
    }

}

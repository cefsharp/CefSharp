// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("CefSharp.Test.CefSharpXunitTestFramework", "CefSharp.Test")]

namespace CefSharp.Test
{
    /// <summary>
    /// Custom <see cref="XunitTestFramework"/> that allows for code to run begin/end of
    /// each test run. Classes should implement <see cref="IDisposable"/>
    /// </summary>
    public class CefSharpXunitTestFramework : XunitTestFramework
    {
        public CefSharpXunitTestFramework(IMessageSink messageSink)
          : base(messageSink)
        {
            //Create a new BindingRedirectAssemblyResolver to resolve the assemblies
            //that xUnit is unable to Load
            DisposalTracker.Add(new BindingRedirectAssemblyResolver());
        }
    }
}

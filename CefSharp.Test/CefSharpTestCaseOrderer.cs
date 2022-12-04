// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CefSharp.Test
{
    public class CefSharpTestCaseOrderer : ITestCaseOrderer
    {
        private readonly IMessageSink diagnosticMessageSink;

        public CefSharpTestCaseOrderer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        IEnumerable<TTestCase> ITestCaseOrderer.OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        {
            var result = testCases.ToList();  // Run them in discovery order

            if(result.Count > 0)
            {
                var firstTestCase = result[0];
                
                var message = new DiagnosticMessage("Ordered Test Cases for : {0} ", firstTestCase.TestMethod.TestClass.Class.ToString());
                diagnosticMessageSink.OnMessage(message);
            }

            return result;
        }
    }
}

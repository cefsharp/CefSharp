// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.ModelBinding;
using System.Collections.Generic;
using Xunit;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// BinderFacts - Tests model default binder behavior
    /// </summary>
    public class BinderFacts
    {
        enum TestEnum
        {
            A,
            B,
            C
        }

        class TestObject
        {
            public string AString;
            public bool ABool;
            public int AnInteger;
            public double ADouble;
            public TestEnum AnEnum;
        }

        [Fact]
        public void BindsComplexObjects()
        {
            var binder = new DefaultBinder(new DefaultFieldNameConverter());
            var obj = new Dictionary<string, object>
            {
                { "AnEnum", 2 },
                { "AString", "SomeValue" },
                { "ABool", true },
                { "AnInteger", 2.4 },
                { "ADouble", 2.6 }
            };

            var result = (TestObject)binder.Bind(obj, typeof(TestObject));

            Assert.Equal(TestEnum.C, result.AnEnum);
            Assert.Equal(obj["AString"], result.AString);
            Assert.Equal(obj["ABool"], result.ABool);
            Assert.Equal(2, result.AnInteger);
            Assert.Equal(obj["ADouble"], result.ADouble);
        }

        [Fact]
        public void BindsEnums()
        {
            var binder = new DefaultBinder(new DefaultFieldNameConverter());
            var result = binder.Bind(2, typeof(TestEnum));

            Assert.Equal(TestEnum.C, result);
        }

        [Fact]
        public void BindsIntegersWithPrecisionLoss()
        {
            var binder = new DefaultBinder(new DefaultFieldNameConverter());
            var result = binder.Bind(2.5678, typeof(int));

            Assert.Equal(3, result);

            result = binder.Bind(2.123, typeof(int));

            Assert.Equal(2, result);
        }

        [Fact]
        public void BindsDoublesWithoutPrecisionLoss()
        {
            const double Expected = 2.5678;
            var binder = new DefaultBinder(new DefaultFieldNameConverter());
            var result = binder.Bind(Expected, typeof(double));

            Assert.Equal(Expected, result);

            result = binder.Bind(2, typeof(double));

            Assert.Equal(2.0, result);
        }
    }
}

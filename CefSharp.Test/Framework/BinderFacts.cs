// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.JavascriptBinding;
using CefSharp.ModelBinding;
using Xunit;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// BinderFacts - Tests model default binder behavior
    /// </summary>
    public class BinderFacts
    {
        private enum TestEnum
        {
            A,
            B,
            C
        }

#pragma warning disable CS0649
        private class TestObject
        {
            public string AString;
            public bool ABool;
            public int AnInteger;
            public double ADouble;
            public TestEnum AnEnum;
        }
#pragma warning restore CS0649

        [Fact]
        public void BindsComplexObjects()
        {
            var binder = new DefaultBinder();
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
            var binder = new DefaultBinder();
            var result = binder.Bind(2, typeof(TestEnum));

            Assert.Equal(TestEnum.C, result);
        }

        [Fact]
        public void BindsIntegersWithPrecisionLoss()
        {
            var binder = new DefaultBinder();
            var result = binder.Bind(2.5678, typeof(int));

            Assert.Equal(3, result);

            result = binder.Bind(2.123, typeof(int));

            Assert.Equal(2, result);
        }

        [Fact]
        public void BindsDoublesWithoutPrecisionLoss()
        {
            const double Expected = 2.5678;
            var binder = new DefaultBinder();
            var result = binder.Bind(Expected, typeof(double));

            Assert.Equal(Expected, result);

            result = binder.Bind(2, typeof(double));

            Assert.Equal(2.0, result);
        }

        [Theory]
        [InlineData(0, typeof(int))]
        [InlineData(0d, typeof(double))]
        [InlineData(false, typeof(bool))]
        public void NullToValueTypeTheory(object excepectedResult, Type conversionType)
        {
            var binder = new DefaultBinder();

            var actualResult = binder.Bind(null, conversionType);

            Assert.Equal(excepectedResult, actualResult);
        }

        [Fact]
        public void BindArrayWithNullElementToIntArray()
        {
            var arrayType = typeof(int[]);

            var binder = new DefaultBinder();
            var obj = new List<object> { 10, 20, null, 30 };
            var result = binder.Bind(obj, arrayType);

            Assert.NotNull(result);
            Assert.Equal(arrayType, result.GetType());

            var arr = (int[])result;
            Assert.Equal(obj.Count, arr.Length);

            for (int i = 0; i < obj.Count; i++)
            {
                var expected = obj[i] ?? 0;
                var actual = arr[i];
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void BindListOfNumbersToDoubleArray()
        {
            var doubleArrayType = typeof(double[]);

            var binder = new DefaultBinder();
            var obj = new List<object> { 10, 20, 1.23 };
            var result = binder.Bind(obj, doubleArrayType);

            Assert.NotNull(result);
            Assert.Equal(doubleArrayType, result.GetType());

            var arr = (double[])result;
            Assert.Equal(obj.Count, arr.Length);

            for (int i = 0; i < obj.Count; i++)
            {
                var expected = Convert.ToDouble(obj[i]);
                var actual = arr[i];
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void HonorsJavascriptNameConverter()
        {
            var namingConverter = new CamelCaseJavascriptNameConverter();
            IBinder binder = new DefaultBinder(namingConverter);
            var obj = new Dictionary<string, object>
            {
                { "aString", "SomeValue" },
                { "aBool", true },
            };

            var result = (TestObject)binder.Bind(obj, typeof(TestObject));

            Assert.Equal(obj["aString"], result.AString);
            Assert.Equal(obj["aBool"], result.ABool);
        }

        [Fact]
        public void HonorsJavascriptNameConverterLegacy()
        {
            var namingConverter = new LegacyCamelCaseJavascriptNameConverter();
            IBinder binder = new DefaultBinder(namingConverter);

            var obj = new Dictionary<string, object>
            {
                { "AString", "SomeValue" },
                { "ABool", true },
            };

            var result = (TestObject)binder.Bind(obj, typeof(TestObject));

            Assert.Equal(obj["AString"], result.AString);
            Assert.Equal(obj["ABool"], result.ABool);
        }
    }
}

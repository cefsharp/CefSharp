// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.JavascriptBinding;
using Xunit;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// LegacyCamelCaseJavascriptNameConverterFacts - Test the different name converters
    /// </summary>
    public class LegacyCamelCaseJavascriptNameConverterFacts
    {
        [Fact]
        public void CanConvertStringToJavascriptName()
        {
            IJavascriptNameConverter converter = new LegacyCamelCaseJavascriptNameConverter();
            var propertyInfo = new TestMemberInfo("APropertyName");

            var result = converter.ConvertToJavascript(propertyInfo);

            Assert.Equal("aPropertyName", result);
        }

        [Fact]
        public void CanConvertStringToReturnedObjectName()
        {
            IJavascriptNameConverter converter = new LegacyCamelCaseJavascriptNameConverter();
            var propertyInfo = new TestMemberInfo("APropertyName");

            var result = converter.ConvertReturnedObjectPropertyAndFieldToNameJavascript(propertyInfo);

            Assert.Equal(propertyInfo.Name, result);
        }
    }
}


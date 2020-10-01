// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Internals;
using Xunit;

namespace CefSharp.Test.Framework
{
    public class PathCheckFacts
    {
        [Fact]
        public void IsPathAbsoluteValid()
        {
            Assert.True(PathCheck.IsAbsolute(@"C:\foo1"));
            Assert.True(PathCheck.IsAbsolute(@"c:\"));
            Assert.True(PathCheck.IsAbsolute(@"C:\foo2"));
            Assert.True(PathCheck.IsAbsolute(@"C:\foo2\"));
            Assert.True(PathCheck.IsAbsolute(@"c:/"));
            Assert.True(PathCheck.IsAbsolute(@"C:/foo1"));
            Assert.True(PathCheck.IsAbsolute(@"c:\"));
            Assert.True(PathCheck.IsAbsolute(@"C:/foo2"));
            Assert.True(PathCheck.IsAbsolute(@"C:\Users\appveyor\AppData\Local\CefSharp\Tests\Cache"));

        }

        [Fact]
        public void IsPathAbsoluteInValid()
        {
            Assert.False(PathCheck.IsAbsolute(@"\"));
            Assert.False(PathCheck.IsAbsolute(@"/"));
            Assert.False(PathCheck.IsAbsolute(@"C:"));
            Assert.False(PathCheck.IsAbsolute(@"."));
            Assert.False(PathCheck.IsAbsolute(@".."));
            Assert.False(PathCheck.IsAbsolute(@"cache"));
            Assert.False(PathCheck.IsAbsolute(@"cache\"));
        }

        [Fact]
        public void AssertPathAbsoluteInValid()
        {
            Assert.Throws<Exception>(() => PathCheck.AssertAbsolute(@"\", "Path"));
            Assert.Throws<Exception>(() => PathCheck.AssertAbsolute(@"c:foo", "Path"));
            Assert.Throws<Exception>(() => PathCheck.AssertAbsolute(@"cache", "Path"));
            Assert.Throws<Exception>(() => PathCheck.AssertAbsolute(@"locales\", "Path"));
        }
    }
}

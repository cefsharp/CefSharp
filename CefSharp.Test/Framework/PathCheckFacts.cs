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
        }

        [Fact]
        public void IsPathAbsoluteInValid()
        {
            Assert.False(PathCheck.IsAbsolute(@"\"));
            Assert.False(PathCheck.IsAbsolute(@"/"));
            Assert.False(PathCheck.IsAbsolute(@"C:foo.txt"));
            Assert.False(PathCheck.IsAbsolute(@"C:"));
            Assert.False(PathCheck.IsAbsolute(@"."));
            Assert.False(PathCheck.IsAbsolute(@".."));
            Assert.False(PathCheck.IsAbsolute(@"cache"));
            Assert.False(PathCheck.IsAbsolute(@"cache\"));
        }

        [Fact]
        public void AssertPathAbsoluteInValid()
        {
            Assert.Throws<Exception>(() => PathCheck.IsAbsolute(@"\"));
            Assert.Throws<Exception>(() => PathCheck.IsAbsolute(@"c:foo"));
            Assert.Throws<Exception>(() => PathCheck.IsAbsolute(@"cache"));
            Assert.Throws<Exception>(() => PathCheck.IsAbsolute(@"locales\"));
        }
    }
}

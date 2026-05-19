// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    /// <summary>
    /// CommandLineArgsMergerTests - Test the merging of command line features
    /// </summary>
    public class CommandLineArgsMergerTests
    {
        private readonly ITestOutputHelper output;

        public CommandLineArgsMergerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ShouldMergeFeatures()
        {
            var expected = "A,B,C";

            var actual = CommandLineArgsMerger.MergeFeatures("A,B", "C");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldAvoidDuplicates()
        {
            var expected = "A,B,C";

            var actual = CommandLineArgsMerger.MergeFeatures("A,B", "B,C");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldTrimWhitespace()
        {
            var expected = "A,B,C";

            var actual = CommandLineArgsMerger.MergeFeatures("A, B", " C ");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldHandleNull()
        {
            var expected = "A,B";

            var actual = CommandLineArgsMerger.MergeFeatures("A,B", null);

            Assert.Equal(expected, actual);
        }
    }
}

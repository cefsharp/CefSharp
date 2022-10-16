// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    public class CefTimeUtilsFacts
    {
        private readonly ITestOutputHelper output;

        public CefTimeUtilsFacts(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(0, "1601-01-01")]
        [InlineData(2650467743999999999, "9999-12-31 23:59:59")]
        [InlineData(759797148870000000, "9999-12-31 23:59:59")]
        public void FromBaseTimeToDateTimeShouldWork(long val, string expectedDateTime)
        {
            var actual = CefTimeUtils.FromBaseTimeToDateTime(val);
            var expected = DateTime.Parse(expectedDateTime).ToLocalTime();

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void FromDateTimeToBaseTimeShouldWorkForMaxValue()
        {
            const long expected = 265046774399999999;
            var maxValueAsUtc = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);

            var actual = CefTimeUtils.FromDateTimeToBaseTime(maxValueAsUtc);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void FromDateTimeToBaseTimeShouldWorkForWindowsEpoch()
        {
            const long expected = 0;
            var utcTime = DateTime.SpecifyKind(new DateTime(1601, 01, 01), DateTimeKind.Utc);

            var actual = CefTimeUtils.FromDateTimeToBaseTime(utcTime);

            Assert.Equal(actual, expected);
        }

        //[Fact]
        //public void FromDateTimeToBaseTimeShouldWorkForMinValue()
        //{
        //    const long expected = 0;
        //    var utcTime = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        //    var actual = CefTimeUtils.FromDateTimeToBaseTime(utcTime);

        //    Assert.Equal(actual, expected);
        //}
    }
}

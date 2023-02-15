// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Framework
{
    public class CefTimeUtilsTests
    {
        private readonly ITestOutputHelper output;

        public CefTimeUtilsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(-50491123200000000, "0001-01-01")]
        [InlineData(-86400000000, "1600-12-31")]
        [InlineData(0, "1601-01-01")]
        [InlineData(2650467743999999999, "9999-12-31 23:59:59")]
        [InlineData(759797148870000000, "9999-12-31 23:59:59")]
        public void FromBaseTimeToDateTimeShouldWork(long val, string expectedDateTime)
        {
            var actual = CefTimeUtils.FromBaseTimeToDateTime(val);
            var expected = DateTime.Parse(expectedDateTime).ToLocalTime();

            Assert.Equal(expected, actual, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void FromDateTimeToBaseTimeShouldWorkForMaxValue()
        {
            const long expected = 265046774399999999;
            var maxValueAsUtc = DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);

            var actual = CefTimeUtils.FromDateTimeToBaseTime(maxValueAsUtc);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromDateTimeToBaseTimeShouldWorkForWindowsEpoch()
        {
            const long expected = 0;
            var utcTime = DateTime.SpecifyKind(new DateTime(1601, 01, 01), DateTimeKind.Utc);

            var actual = CefTimeUtils.FromDateTimeToBaseTime(utcTime);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromDateTimeToBaseTimeShouldWorkForMinValue()
        {
            const long expected = -50491123200000000;
            var utcTime = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

            var actual = CefTimeUtils.FromDateTimeToBaseTime(utcTime);

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ShouldConvertMinValueToAndFromDateTime()
        {
            var expected = DateTime.MinValue.ToLocalTime();

            var baseTime = CefTimeUtils.FromDateTimeToBaseTime(expected);

            var actual = CefTimeUtils.FromBaseTimeToDateTime(baseTime);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldConvertMaxValueToAndFromDateTime()
        {
            var expected = DateTime.MaxValue.ToLocalTime();

            var baseTime = CefTimeUtils.FromDateTimeToBaseTime(expected);

            var actual = CefTimeUtils.FromBaseTimeToDateTime(baseTime);

            Assert.Equal(expected, actual, TimeSpan.FromMilliseconds(10));
        }
    }
}

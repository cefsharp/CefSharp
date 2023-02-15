// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.DevTools.Network;
using Xunit;

namespace CefSharp.Test.DevTools
{
    public class HeadersTests
    {
        [Fact]
        public void HeadersTryGetValues()
        {
            var headers = new Headers();
            headers["EMPTY"] = " ";
            headers["SIMPLE"] = " 1 ";
            headers["MULTIPLE"] = " 1 , \" 1,5 \",2, \" 2 , 5 \" ,  3  , ,";

            string[] values;

            Assert.False(headers.TryGetValues("doesnotexist", out values));
            Assert.Null(values);

            Assert.True(headers.TryGetValues("EMPTY", out values));
            Assert.Collection(values,
                v => Assert.Equal("", v)
            );

            Assert.True(headers.TryGetValues("SIMPLE", out values));
            Assert.Collection(values,
                v => Assert.Equal("1", v)
            );

            Assert.True(headers.TryGetValues("MULTIPLE", out values));
            Assert.Collection(values,
                v => Assert.Equal("1", v),
                v => Assert.Equal(" 1,5 ", v),
                v => Assert.Equal("2", v),
                v => Assert.Equal(" 2 , 5 ", v),
                v => Assert.Equal("3", v),
                v => Assert.Equal("", v),
                v => Assert.Equal("", v)
            );
        }

        [Fact]
        public void HeadersGetCommaSeparatedValues()
        {
            var headers = new Headers();
            headers["EMPTY"] = " ";
            headers["SIMPLE"] = " 1 ";
            headers["MULTIPLE"] = " 1 , \" 1,5 \",2, \" 2 , 5 \" ,  3  , ,";

            Assert.Null(headers.GetCommaSeparatedValues("doesnotexist"));
            Assert.Collection(headers.GetCommaSeparatedValues("EMPTY"),
                v => Assert.Equal("", v)
            );
            Assert.Collection(headers.GetCommaSeparatedValues("SIMPLE"),
                v => Assert.Equal("1", v)
            );
            Assert.Collection(headers.GetCommaSeparatedValues("MULTIPLE"),
                v => Assert.Equal("1", v),
                v => Assert.Equal(" 1,5 ", v),
                v => Assert.Equal("2", v),
                v => Assert.Equal(" 2 , 5 ", v),
                v => Assert.Equal("3", v),
                v => Assert.Equal("", v),
                v => Assert.Equal("", v)
            );
        }

        [Fact]
        public void HeadersAppendCommaSeparatedValues()
        {
            var headers = new Headers();
            headers.AppendCommaSeparatedValues("TEST", " 1 ");

            headers.AppendCommaSeparatedValues("test", "\" 1,5 \"", "2", " 2 , 5 ", "3", " ", "");

            Assert.Collection(headers.GetCommaSeparatedValues("Test"),
                v => Assert.Equal("1", v),
                v => Assert.Equal(" 1,5 ", v),
                v => Assert.Equal("2", v),
                v => Assert.Equal(" 2 , 5 ", v),
                v => Assert.Equal("3", v),
                v => Assert.Equal("", v),
                v => Assert.Equal("", v)
            );
        }

        [Fact]
        public void HeadersSetCommaSeparatedValues()
        {
            var headers = new Headers();
            headers["TEST"] = " 1 ";

            headers.SetCommaSeparatedValues("test", "\" 1,5 \"", "2", " 2 , 5 ", "3", " ", "");

            Assert.Collection(headers.GetCommaSeparatedValues("Test"),
                v => Assert.Equal(" 1,5 ", v),
                v => Assert.Equal("2", v),
                v => Assert.Equal(" 2 , 5 ", v),
                v => Assert.Equal("3", v),
                v => Assert.Equal("", v),
                v => Assert.Equal("", v)
            );
        }

    }
}

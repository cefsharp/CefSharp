// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;
using CefSharp.Wpf.Internals;
using Xunit;

namespace CefSharp.Test.Wpf
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    /// <summary>
    /// Issue #4021
    /// Validate Cursor resource Ids
    /// </summary>
    [Collection(CefSharpFixtureCollection.Key)]
    public class EmbeddedCursorLoadTests
    {
        [Theory]
        [InlineData(CursorType.Alias)]
        [InlineData(CursorType.Cell)]
        [InlineData(CursorType.ColumnResize)]
        [InlineData(CursorType.Copy)]
        [InlineData(CursorType.Grab)]
        [InlineData(CursorType.Grabbing)]
        [InlineData(CursorType.EastPanning)]
        [InlineData(CursorType.MiddlePanning)]
        [InlineData(CursorType.MiddlePanningHorizontal)]
        [InlineData(CursorType.MiddlePanningVertical)]
        [InlineData(CursorType.NorthPanning)]
        [InlineData(CursorType.NortheastPanning)]
        [InlineData(CursorType.NorthwestPanning)]
        [InlineData(CursorType.SouthPanning)]
        [InlineData(CursorType.SoutheastPanning)]
        [InlineData(CursorType.SouthwestPanning)]
        [InlineData(CursorType.WestPanning)]
        [InlineData(CursorType.RowResize)]
        [InlineData(CursorType.VerticalText)]
        [InlineData(CursorType.ZoomIn)]
        [InlineData(CursorType.ZoomOut)]
        public void CanGetEmbeddedCursors(CursorType cursorType)
        {
            var actual = EmbeddedCursor.TryLoadCursor(cursorType, out IntPtr cursorPtr);

            Assert.True(actual);
            Assert.NotEqual(IntPtr.Zero, cursorPtr);
        }
    }
}

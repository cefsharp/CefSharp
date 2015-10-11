// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public struct DraggableRegion
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Draggable { get; private set; }

        public DraggableRegion(int width, int height, int x, int y, bool draggable) : this()
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
            Draggable = draggable;
        }
    }
}

// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    public struct ViewRect
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ViewRect(int width, int height) : this()
        {
            Width = width;
            Height = height;
        }
    }
}


// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    public ref class NativeMethodWrapper sealed
    {
    public:
        static void CopyMemoryUsingHandle(IntPtr dest, IntPtr src, int numberOfBytes);
        static bool IsFocused(IntPtr handle);
        static void SetWindowPosition(IntPtr handle, int x, int y, int width, int height);
        static void SetWindowParent(IntPtr child, IntPtr newParent);
    };
}
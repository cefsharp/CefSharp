// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    /// <exclude />
    public ref class NativeMethodWrapper sealed
    {
    public:
        //Method cannot be called CopyMemory/RtlCopyMemroy as that's a macro name
        //Length is currently int, update if required to handle larger data structures
        //(int is plenty big enough for our current use case)
        static void MemoryCopy(IntPtr dest, IntPtr src, int numberOfBytes);
        static bool IsFocused(IntPtr handle);
        static void SetWindowPosition(IntPtr handle, int x, int y, int width, int height);
        static void SetWindowParent(IntPtr child, IntPtr newParent);
        static void RemoveExNoActivateStyle(IntPtr browserHwnd);
    };
}

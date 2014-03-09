// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

namespace CefSharp
{
    public ref class BitmapInfo
    {
    internal:
        property Object^ BitmapLock;
        property HANDLE BackBufferHandle;
    public:
        property bool IsPopup;
        property int Width;
        property int Height;
        
        property IntPtr FileMappingHandle;

        // Cannot be InteropBitmap since we really don't want CefSharp to be dependent on WPF libraries.
        Object^ InteropBitmap;

        BitmapInfo()
        {
            BitmapLock = gcnew Object();
        };
    };
}

// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IWebBrowserInternal.h"

namespace CefSharp
{
    private interface class IRenderWebBrowser : IWebBrowserInternal
    {
        property IntPtr FileMappingHandle;
        property int BytesPerPixel { int get(); };

        property int Width { int get(); };
        property int Height { int get(); };

        void InvokeRenderAsync(Action^ callback);

        void SetCursor(IntPtr cursor);

        void ClearBitmap();
        void ClearPopupBitmap();
        void SetBitmap();
        void SetPopupBitmap();
    };
}
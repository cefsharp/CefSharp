// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "IWebBrowser.h"

namespace CefSharp
{
    public interface class IRenderWebBrowser : IWebBrowser
    {
        property IntPtr FileMappingHandle;
        property IntPtr PopupFileMappingHandle;
        property int BytesPerPixel { int get(); };

        void InvokeRenderAsync(Action^ callback);

        void SetCursor(IntPtr cursor);

        void SetBitmap();

        void SetPopupBitmap();
        void SetPopupIsOpen(bool isOpen);
        void SetPopupSizeAndPosition(IntPtr rect);
    };
}
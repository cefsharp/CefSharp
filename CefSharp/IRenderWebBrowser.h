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
        void SetCursor(IntPtr cursor);
        void SetBuffer(int width, int height, const void* buffer);

        void SetPopupBuffer(int width, int height, const void* buffer);
        void SetPopupIsOpen(bool isOpen);

        void SetPopupSizeAndPosition(const void* rect);
    };
}
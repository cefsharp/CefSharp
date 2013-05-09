#include "Stdafx.h"
#pragma once

#include "IWebBrowser.h"

namespace CefSharp
{
    public interface class IRenderWebBrowser : IWebBrowser
    {
    public:
        void SetCursor(IntPtr cursor);
        void SetBuffer(int width, int height, const void* buffer);

        void SetPopupBuffer(int width, int height, const void* buffer);
        void SetPopupIsOpen(bool isOpen);

        void SetPopupSizeAndPosition(const void* rect);
    };
}
#include "stdafx.h"
#pragma once

#include "IWebBrowser.h"

namespace CefSharp
{
    public interface class IOffscreenWebBrowser : IWebBrowser
    {
    public:
        void SetCursor(CefCursorHandle cursor);
        void SetBuffer(int width, int height, const std::vector<CefRect>& dirtyRects, const void* buffer);
    };
}